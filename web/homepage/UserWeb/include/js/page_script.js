/*
######################################################################################################
'
' File name     : page_script.js
' Location			:
' Author        :
' Create date 	:
' Last modify 	:
' Description  	: 페이지템플릿 스크립트
'######################################################################################################
*/
//게임목록이동.
function uf_GoGameList(no){
	document.location.href="/game/game_list.asp?game_group="+no;
}
//포인트전환
function uf_PointChange(){
	document.HiddenFrame.location.href="/mypage/point_change_proc.asp";
}
//즐겨찾기
function uf_AddFavor(title,url) {
   if (window.sidebar) // firefox    
   	window.sidebar.addPanel(title, url, ""); 
   else if(window.opera && window.print)
   { // opera       
   		var elem = document.createElement('a'); 
      elem.setAttribute('href',url); 
      elem.setAttribute('title',title); 
      elem.setAttribute('rel','sidebar'); 
      elem.click(); 
   } 
   else if(document.all) // ie
   window.external.AddFavorite(url, title);
}
// 게임목록새로고침
function uf_DoReload() {	
	uf_NowMemberCheck();
	try{
		uf_GameReflash();
	}catch(e){}		
}

// 현재접속정보 기록
function uf_NowMemberCheck(){
	//현재접속시간기록.
	$.ajax({
		type: "POST",
		url: "/common/ajax_now_member_check.asp",
		async: false,
		dataType:"xml",
		data:"",
		success: function (xml) {
			if($(xml).find("result").length > 0){
				$(xml).find("result").each(function(){ 
          var result_code = $(this).find("result_code").text();
					//alert(result_code);	         	
     		});
   		}
 		}
 		,error: function(){ alert("ajax error!!"); document.location.reload();}
	});
}

// 팝업공지 관련 스크립트
var box_name="PopNotice";
		
//팝업열기
function uf_noti_popup_open(){	
	//오늘 공지숨기기 쿠키값이 없으면 팝업오픈.
	if(getCookie("main_notice_popup") != "done"){
		//z-index 최상위로 올리기
		zIndex=zIndex + 1;
		$('#'+box_name).css('z-index', zIndex);
		
		//화면중앙정렬.
		jquery_locationcenter(box_name);
					
		// 화면출력
		$('#'+box_name).show('fast');
	}
}

//12시간 동안 팝업닫기.
function uf_noti_today_off(hh){
	setCookie("main_notice_popup", "done" , 1); // 오른쪽 숫자는 쿠키를 유지할 기간을 설정합니다
	//setCookieHour("main_notice_popup", "done" , hh); //시간단위 (12시간)
	$('#'+box_name).hide();
}

//팝업닫기
function uf_noti_off(){
	$('#'+box_name).hide();	
}


//신규쪽지알림팝업.
function uf_newmemo_popup_open(){	
	var box_name="PopNewMemo";

	//z-index 최상위로 올리기
	zIndex=zIndex + 1;
	$('#'+box_name).css('z-index', zIndex);
	
	//화면중앙정렬.
	jquery_locationcenter(box_name);
				
	// 화면출력
	$('#'+box_name).show('fast');
}
//신규쪽지알림팝업.닫기
function uf_popup_off(box_name){
	$('#'+box_name).hide();	
}