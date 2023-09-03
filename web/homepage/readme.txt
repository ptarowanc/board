- 개발 환경
프레임워크 : ASP.NET MVC 5


- 프로젝트 설명
AdminWeb : 관리자 페이지입니다. (수정X)
DBLIB : 데이터베이스 라이브러리입니다. (수정X)
UserWeb : 홈 페이지입니다.


- 디렉토리 설명
common, content, include, Scripts : css, js, 부트스트랩 등이 있는 폴더입니다.
images : 이미지 파일이 있는 폴더입니다.
Views : 페이지 코드입니다.


- 작업 설명
UserWeb의 Views 디렉토리에 cshtml 파일을 디자인 요구사항에 맞게 수정해주시면 됩니다.
ASP.NET MVC에서 cshtml 파일은 Razor구문이 적용됩니다.


- 그 외
처음, nuget에서 패키지를 복구하신 후 빌드하시기 바랍니다.

빌드한 프로젝트를 웹브라우저로 보는방법은 비쥬얼 스튜디오내에서 디버깅을 하거나, 솔루션 탐색기에서 보려는 페이지 우클릭 -> 브라우저에서 보기를 하시면 됩니다.

데이터베이스는 외부경로에 접속해서 통신합니다.

Razor의 자세한 사용방법은 Microsoft Docs를 참조해주세요. https://docs.microsoft.com/ko-kr/aspnet/web-pages/overview/getting-started/introducing-razor-syntax-c

홈페이지에서 로그인할 수 있는 계정입니다. 계정: t1, 비번: 1


