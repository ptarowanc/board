/* vim: set softtabstop=2 shiftwidth=2 expandtab : */
#ifndef RAD_ZIP_H
#define RAD_ZIP_H

typedef struct {
  char *filename;
  unsigned long long file_pos;
  unsigned short method;
} rrZipFile_t;

typedef struct {
  rrZipFile_t *files;
  unsigned long long num_files;
} rrZip_t;

#ifdef __cplusplus
extern "C" {
#endif

rrZip_t rr_zip_read(const char *filename);
void rr_zip_free(rrZip_t *z);

#ifdef __cplusplus
}
#endif

#endif // RAD_ZIP_H

#ifndef RAD_ZIP_HEADER_FILE_ONLY

#ifndef RAD_ZIP_MALLOC
#define RAD_ZIP_MALLOC malloc
#endif

#ifndef RAD_ZIP_FREE
#define RAD_ZIP_FREE free
#endif

#include <stdlib.h>
#include <stdio.h>
#include <string.h>

#pragma pack(push,1)
typedef struct {
  unsigned magic; // 0x06054B50
  unsigned short disk;
  unsigned short disk_cd_start;
  unsigned short num_files;
  unsigned short num_files_total;
  unsigned size_cd;
  unsigned offset_cd;
  unsigned short size_comment;
} zipDirectoryInfo_t;

typedef struct {
  unsigned magic; // 0x06064B50
  unsigned long long size_end_cd;
  unsigned short version_made_by;
  unsigned short version_needed;
  unsigned disk;
  unsigned disk_cd_start;
  unsigned long long num_files;
  unsigned long long num_files_total;
  unsigned long long size_cd;
  unsigned long long offset_cd;
} zip64DirectoryInfo_t;

typedef struct {
  unsigned magic; // 0x07064B50
  unsigned disk;
  unsigned long long offset_ecd;
  unsigned num_disks;
} zip64EndOfCD_t;

typedef struct { 
  unsigned magic; // 0x02014B50
  unsigned short version;
  unsigned short version_needed;
  unsigned short flag;
  unsigned short method;
  unsigned dos_date;
  unsigned crc;
  unsigned compressed_size;
  unsigned uncompressed_size;
  unsigned short size_filename;
  unsigned short size_extra;
  unsigned short size_comment;
  unsigned short disk;
  unsigned short internal_attr;
  unsigned external_attr;
  unsigned offset; 
} zipFileInfo_t;

typedef struct {
  unsigned magic; // 0x04034B50
  unsigned short version_needed;
  unsigned short flags;
  unsigned short method;
  unsigned last_mod_time;
  unsigned crc;
  unsigned compressed_size;
  unsigned uncompressed_size;
  unsigned short size_filename;
  unsigned short size_extra;
} zipFileLocalInfo_t;
#pragma pack(pop)

// Note: assumes zip file itself is < 2GB (easily fixed if necessary), though output can be bigger
rrZip_t rr_zip_read(const char *filename) {
  unsigned char buf[0x404];
  long start, end, at;
  int isZip64 = 0;
  unsigned long long dirAt = 0;
  unsigned i;
  rrZip_t rrzip = {0};
  zip64DirectoryInfo_t cd = {0};
  FILE *fp;
  rrZipFile_t *rzinfos;
  unsigned long long offset;
  
  fp = fopen(filename, "rb");
  if(!fp) {
    return rrzip;
  }

  fseek(fp, 0, SEEK_END);
  end = ftell(fp);
  start = end - 0xFFFF;
  start = start < 0 ? 0 : start;

  for(at = end - 0x400; at > start; at -= 0x400) {
    fseek(fp, at, SEEK_SET);
    if ( fread(buf, 1, 0x404, fp) < 0x400 ) goto nope;
    // Fast Horspool-esque search for the central directory
    // Note: cannot reverse search as zip64 has a bogus zip32 header beacon.
    for(i = 0; i < 0x400; ++i) {
      int v = *(int*)(buf+i);
      if(v == 0x06054B50) {
        dirAt = at + i;
        goto found_dir;
      } 
      if(v == 0x07064B50) {
        dirAt = at + i;
        isZip64 = 1;
        goto found_dir;
      }
    }
  }
 nope:
  fclose(fp);
  return rrzip;

found_dir:
  fseek(fp, (long)dirAt, SEEK_SET);

  if(isZip64) {
    zip64EndOfCD_t ecd;
    if ( fread(&ecd, 1, sizeof(ecd), fp) != sizeof(ecd) ) goto nope;
    dirAt = ecd.offset_ecd;
    fseek(fp, (long)dirAt, SEEK_SET);
    if ( fread(&cd, 1, sizeof(cd), fp) != sizeof(cd) ) goto nope;
    if(cd.magic != 0x06064B50) {
      // bad file;
      return rrzip;
    }
  } else {
    // Grab central dir info 
    zipDirectoryInfo_t cd32;
    if ( fread(&cd32, 1, sizeof(cd32), fp) != sizeof(cd32) ) goto nope;

    cd.magic = cd32.magic;
    cd.size_end_cd = 0;
    cd.version_made_by = 0;
    cd.version_needed = 0;
    cd.disk = cd32.disk;
    cd.disk_cd_start = cd32.disk_cd_start;
    cd.num_files = cd32.num_files;
    cd.num_files_total = cd32.num_files_total;
    cd.size_cd = cd32.size_cd;
    cd.offset_cd = cd32.offset_cd;
  }

  rzinfos = (rrZipFile_t*)RAD_ZIP_MALLOC((size_t)(sizeof(rrZipFile_t)*cd.num_files));
  if(!rzinfos) {
    // bad file;
    return rrzip;
  }
  memset(rzinfos, 0, (size_t)(sizeof(rrZipFile_t)*cd.num_files));

  // Grab files
  fseek(fp, (long)(dirAt - cd.size_cd), SEEK_SET);
  for(i = 0; i < cd.num_files; ++i) {
    zipFileInfo_t info;
    if ( fread(&info, 1, sizeof(info), fp) != sizeof(info) ) goto nope;
    if(info.magic != 0x02014B50) {
      goto hell;
    }
    rzinfos[i].filename = (char*)RAD_ZIP_MALLOC(info.size_filename+1);
    if ( fread(rzinfos[i].filename, 1, info.size_filename, fp) != info.size_filename ) goto nope;
    rzinfos[i].filename[info.size_filename] = 0;
    if(info.offset == 0xFFFFFFFF) {
      unsigned short magic;
      int skip;
      if ( fread(&magic, 1, 2, fp) != 2 ) goto nope;
      if(magic != 1) { // Zip64 header magic
        goto hell;
      }
      skip = 2;
      if(info.compressed_size == 0xFFFFFFFF) {
        skip += 8;
      }
      if(info.uncompressed_size == 0xFFFFFFFF) {
        skip += 8;
      }
      fseek(fp, skip, SEEK_CUR);
      if ( fread(&offset, 1, 8, fp) != 8 ) goto nope;
      skip = info.disk == 0xFFFF ? 4 : 0;
      fseek(fp, skip+info.size_comment, SEEK_CUR);
    } else {
      offset = info.offset;
      fseek(fp, info.size_extra+info.size_comment, SEEK_CUR);
    }
    rzinfos[i].file_pos = dirAt - cd.offset_cd - cd.size_cd + offset;
    rzinfos[i].method = info.method;
  }

  // Grab file offsets
  for(i = 0; i < cd.num_files; ++i) {
    zipFileLocalInfo_t info;
    fseek(fp, (long)rzinfos[i].file_pos, SEEK_SET);
    if ( fread(&info, 1, sizeof(info), fp) != sizeof(info) ) goto nope;
    if(info.magic != 0x04034B50) {
      goto hell; 
    }
    rzinfos[i].file_pos += 0x1E + info.size_filename + info.size_extra;
  }

  fclose(fp);
  rrzip.num_files = cd.num_files;
  rrzip.files = rzinfos;
  return rrzip;
hell:
  fclose(fp);
  rrzip.num_files = cd.num_files;
  rrzip.files = rzinfos;
  rr_zip_free(&rrzip);
  return rrzip;
}

void rr_zip_free(rrZip_t *z) {
  unsigned i;
  if(!z) {
    return;
  }
  for(i = 0; i < z->num_files; ++i) {
    if(z->files[i].filename) {
      RAD_ZIP_FREE(z->files[i].filename);
      z->files[i].filename = 0;
    }
  }
  if(z->files) {
    RAD_ZIP_FREE(z->files);
    z->files = 0;
  }
  z->num_files = 0;
}

/*
int main(int argc, char *argv[]) {
  unsigned i;
  rrZip_t rrzip = rr_zip_read(argv[1]);
  for(i = 0; i < rrzip.num_files; ++i) {
    printf("Found '%s' at %llx with method %i\n", rrzip.files[i].filename, rrzip.files[i].file_pos, rrzip.files[i].method);
  }
  rr_zip_free(&rrzip);
  return 0;
}
//*/

#endif

