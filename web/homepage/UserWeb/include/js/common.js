/*
######################################################################################################
' File name     : Common.js
' Description  	: 공통 자바스크립트 함수정의
'######################################################################################################
*/
// //컨트롤키, 마우스오른쪽 금지소스
//document.onkeydown=KeyEventHandle;
//document.onkeyup=KeyEventHandle;
//document.oncontextmenu = MouseEventHandle;
//
//function KeyEventHandle() {
//	if((event.ctrlKey == true && (event.keyCode == 78 || event.keyCode == 82)) || (event.keyCode >= 112 && event.keyCode <= 123)) {
//	 event.keyCode = 0;
//	 event.cancelBubble = true;
//	 event.returnValue = false;
//	}
//}
//function MouseEventHandle() {
//	return false;
//}

//++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
//브라우저 버전체크
//++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
var sUserAgent = navigator.userAgent;
var isOpera = sUserAgent.indexOf("Opera") > -1;
var isIE = sUserAgent.indexOf("MSIE") > -1 || sUserAgent.indexOf("rv:") > -1;

var zIndex=100;
var mouse_x,mouse_y;

//++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
//중앙 팝업
//pop_win("win", "http://", 0, 0, 300, 200, 0, 0, 0, 0, 0); 
//++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
function pop_win(name, url, left, top, width, height, toolbar, menubar, statusbar, scrollbar, resizable)
{
	toolbar_str = toolbar ? 'yes' : 'no';
	menubar_str = menubar ? 'yes' : 'no';
	statusbar_str = statusbar ? 'yes' : 'no';
	scrollbar_str = scrollbar ? 'yes' : 'no';
	resizable_str = resizable ? 'yes' : 'no';
	
	var left = (screen.width - width) / 2;
	var top  = (screen.height - height) / 2;
	winprops = 'left='+left+',top='+top+',width='+width+',height='+height+',toolbar='+toolbar_str+',menubar='+menubar_str+',status='+statusbar_str+',scrollbars='+scrollbar_str+',resizable='+resizable_str
	win = window.open(url, name, winprops)
	//if (parseInt(navigator.appVersion) >= 4) { win.window.focus(); }
}

//++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
//윈도우창본문내용에 맞게 리사이즈
//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
function WinResize() {
	var Dwidth = parseInt(document.body.scrollWidth);
	var Dheight = parseInt(document.body.scrollHeight);
	var divEl = document.createElement("div");
	
	divEl.style.position = "absolute";
	divEl.style.left = "0px";
	divEl.style.top = "0px";
	divEl.style.width = "100%";
	divEl.style.height = "100%";
	
	document.body.appendChild(divEl);
	//document.body.insertBefore(divEl, document.body.firstChild);	
	//alert("Dwidth : " + Dwidth + ", divEl.offsetWidth : " + divEl.offsetWidth + ",Dheight : " + Dheight + ", divEl.offsetHeight : " + divEl.offsetHeight);	
	if(navigator.userAgent.indexOf("MSIE") !=-1)
		if (navigator.userAgent.indexOf("MSIE 6") > 0){			
		}else{
			window.resizeBy(Dwidth-divEl.offsetWidth, Dheight-divEl.offsetHeight + 10);
		}
	else
		window.resizeBy(Dwidth-divEl.offsetWidth, Dheight-divEl.offsetHeight);
	document.body.removeChild(divEl);
}

//++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
//윈도우창 화면 가운데 위치 -- 팝업페이지 삽입 <body onload="WinCenter();">
//++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
function WinCenter() {
	if (document.layers) {
		var s_width = screen.width / 2 - outerWidth / 2;
		var t_height = screen.height / 2 - outerHeight / 2;
	} else {
		var s_width = screen.width / 2 - document.body.offsetWidth / 2 + 20;
		var t_height = screen.height / 2 - document.body.offsetHeight / 2 + 20;
	}
	self.moveTo(s_width, t_height);
}

//++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
//이미지를 새창에서보기. (테두리배경안에 이미지표시)
//++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
function ViewPhoto(url,title) { 
var imgwin = window.open("about:blank","photo_win","scrollbars=1,status=0,toolbar=0,resizable=0,location=no,menu=no,width=200,height=200"); 
imgwin.focus(); 
imgwin.document.open();
imgwin.document.write("<title>"+title+"</title>\n"); 
imgwin.document.write("<sc"+"ript type='text/javascript' src='/include/js/jquery-1.8.1.min.js'></sc"+"ript>\n");
imgwin.document.write("<sc"+"ript type='text/javascript' src='/include/js/common.js'></sc"+"ript>\n");
imgwin.document.write("<body topmargin='0' leftmargin='0' bgcolor='#F1F1F1'>\n");
imgwin.document.write("<table id='BodyContent' cellpadding='1' cellspacing='1' bgcolor='#DDDDDD'>\n");
imgwin.document.write("  <tr><td valign='top' bgcolor='#F1F1F1' style='padding: 5 5 5 5;'>\n");
imgwin.document.write("	 <img src='"+url+"' border='0' onclick='self.close()' style='cursor:pointer;' title='이미지를 클릭하시면 닫힙니다'>\n");
imgwin.document.write("	 </td></tr>\n");
imgwin.document.write("</table>\n");
imgwin.document.write("<sc"+"ript type='text/javascript'>\n"); 
imgwin.document.write("$(document).ready(function(){	\n");
imgwin.document.write("WinResize();\n");
imgwin.document.write("WinCenter();\n");
imgwin.document.write("});\n");
imgwin.document.write("</sc"+"ript>\n");
imgwin.document.write("</body>\n");
imgwin.document.close(); 
}

//++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
//이미지리사이즈함수
//++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
function imgRsize(img, rW, rH){
        var iW = img.width;
        var iH = img.height;
        //alert(iW);
        var g = new Array;
        if(iW < rW && iH < rH) { // 가로세로가 축소할 값보다 작을 경우
                g[0] =  iW;
                g[1] =  iH;
        } else {
                if(img.width > img.height) { // 원크기 가로가 세로보다 크면

                        g[0] = rW;
                        g[1] = Math.ceil(img.height * rW / img.width);
                } else if(img.width <= img.height) { //원크기의 세로가 가로보다 크면
                        g[0] = Math.ceil(img.width * rH / img.height);
                        g[1] = rH;
                } else {
                        g[0] = rW;
                        g[1] = rH;
                }
                if(g[0] > rW) { // 구해진 가로값이 축소 가로보다 크면

                        g[0] = rW;
                        g[1] = Math.ceil(img.height * rW / img.width);
                }
                if(g[1] > rH) { // 구해진 세로값이 축소 세로값가로보다 크면
                        g[0] = Math.ceil(img.width * rH / img.height);
                        g[1] = rH;
                }
        }
        g[2] = img.width; // 원사이즈 가로
        g[3] = img.height; // 원사이즈 세로
        return g;
}

//이미지리사이즈함수
function img_Rsize(img, ww, hh){
	//alert("aaa");
    var tt = imgRsize(img, ww, hh);
    if(img.width > ww || img.height > hh){
        img.width = tt[0];
        img.height = tt[1];
    }
}

//사용법 : onLoad="javascript:img_Rsize(img,'가로제한값','세로제한값');"

//++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
//체크박스, 라디오버튼에서 선택한 항목이 있는지 검사함
//++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
function hasCheckedRadio(input) {
 if (input.length > 1) {
 	for (var inx = 0; inx < input.length; inx++) {
 		if (input[inx].checked) return true;
 	}
 } else {
 	if (input.checked) return true;
 }
 return false;
}
function hasCheckedBox(input) {
 	return hasCheckedRadio(input);
}

//++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
//입력박스에 글을 작성할 경우 작성글의 바이트수를 표시함.
//++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
function reCount(str){
	var i;
	var msglen=0;

	for(i=0;i<str.length;i++){
		var ch=str.charAt(i);

		if(escape(ch).length >4){
			msglen += 2;
		}else{
			msglen++;
		}
	}
	return msglen;
}

function nCutMsg(str,nbyte){
	var ret="";
	var i;
	var msglen=0;
	for(i=0;i<str.length;i++){
		var ch=str.charAt(i);
		if(escape(ch).length >4){
			msglen += 2;
		}else{
			msglen++;
		}
		if(msglen > nbyte) break;
		ret += ch;
	}
	return ret;
}

function nByteCheck(ntag1,ntag2,nbyte){
	var obj1 = document.getElementById(ntag1);
	var obj2 = document.getElementById(ntag2); 
	var text = obj1.value;
	var msglen=0;

	msglen = reCount(text);

	if(msglen > nbyte){
		alert('내용은 '+nbyte+'byte 이내로 입력해주세요.');
		text = nCutMsg(text,nbyte);
		obj1.value=text;               
		msglen = reCount(text);
		obj2.innerHTML = "<font color=red>"+ msglen + "</font>/"+nbyte+" byte";
	}else{
		obj2.innerHTML = "<font color=red>"+ msglen + "</font>/"+nbyte+" byte";
	}
}

/*
사용예.
<span class='select' id='res2'>0/2000byte</span>
<textarea onKeyPress=javascript:nByteCheck('leaveWhy','res2',2000); 
	onKeyDown=javascript:nByteCheck('leaveWhy','res2',2000); 
	onKeyUp=javascript:nByteCheck('leaveWhy','res2',2000); 
	onChange=javascript:nByteCheck('leaveWhy','res2',2000); 
	cols='' rows='' id='leaveWhy' name='leaveWhy'></textarea>
*/

function nByteCheckAlert(ntag1,nbyte){
	var obj1 = document.getElementById(ntag1);
	var text = obj1.value;
	var msglen=0;

	msglen = reCount(text);
	//alert(msglen);
	if(msglen > nbyte){
		//alert('내용은 '+nbyte+'byte 이내로 입력해주세요.');
		uf_MakeAlertBox("SystemAlert",1,"안내","내용은 "+nbyte+"byte 이내로 입력해주세요.","","","./images/popup/title_noti.gif");	
		text = nCutMsg(text,nbyte);
		obj1.value=text;
		return false;
	}
}

//++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
// 숫자만 입력받기
//++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
function fnOnlyNumber()
{
	if(event.keyCode < 48 || event.keyCode > 57)
	event.keyCode = null;
}

//++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
// 숫자만 입력받기
//++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
function fnChkNumber(arg)
{
	var val;
	val = arg.value;

	for(i = 0; i < val.length; i++){
		if (val.charAt(i)<'0' || val.charAt(i)>'9'){
			alert("숫자만 입력 하십시오.") ;
			//uf_MakeAlertBox("SystemAlert",1,"안내","숫자만 입력 하십시오.","","","./images/popup/title_noti.gif");	
			arg.value="";
			break;
			return ;
		}
	}
}

/*
사용방법
<input name="inNumber" onKeyPress="fnOnlyNumber();" onKeyUp="fnChkNumber(this);" style="ime-mode: disabled;">
*/

//++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
//페이지내 포커스없애기
//++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
function autoBlur(){ 
	if(event.srcElement.tagName=="A"||event.srcElement.tagName=="/img"){
		document.body.focus(); 
	}
}
document.onfocusin=autoBlur; 

//++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
//동적 플래쉬파일실행
//++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
function FlashFilePlay(filename,w,h){
document.write("<object classid='clsid:D27CDB6E-AE6D-11cf-96B8-444553540000' codebase='http://active.macromedia.com/flash4/cabs/swflash.cab#version=4,0,0,0' width='"+w+"' height='"+h+"'>");
document.write("<param name='movie' value='"+filename+"'>");
document.write("<param name='wmode' Value='Transparent'>");
document.write("<param name='play' value='true'>");
document.write("<param name='loop' value='true'>");
document.write("<param name='quality' value='high'>");
document.write("<embed src='"+filename+"' play='true' loop='true' quality='high' pluginspage='http://www.macromedia.com/shockwave/download/index.cgi?P1_Prod_Version=ShockwaveFlash' width='"+w+"' height='"+h+"' wmode='Transparent'></embed>");
document.write("</object>");
}

//++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
//ajax 모듈
//++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
var ajax={};
ajax.xhr={};

ajax.xhr.Request = function(url, params, callback, method){
	this.url = url;
	this.params = params;
	this.callback = callback;
	this.method = method;
	this.send();
}
ajax.xhr.Request.prototype = {
	getXMLHttpRequest:function(){
		if(window.ActiveXObject){
			try{
				return new ActiveXObject("Msxml2.XMLHTTP");
			}catch(e){
				try{
					return new ActiveXObject("Microsoft.XMLHTTP");
				}catch(e1){return null;}
			}
		}else if(window.XMLHttpRequest){
			return new XMLHttpRequest();
		}else{
			return null;
		}
	},
	send:function(){
		this.req=this.getXMLHttpRequest();
		
		var httpMethod = this.method?this.method:'GET';
		if(httpMethod!='GET'&&httpMethod!='POST'){
			httpMethod='GET';
		}
		var httpParams=(this.params==null||this.params=='')?null:this.params;
		var httpUrl=this.url;
		if(httpMethod=='GET'&&httpParams!=null){
			httpUrl=httpUrl+"?"+httpParams;
		}
		this.req.open(httpMethod, httpUrl, true);
		this.req.setRequestHeader('Content-Type','application/x-www-form-urlencoded');
		var request = this;
		this.req.onreadystatechange=function(){
			request.onStateChange.call(request);
		}
		this.req.send(httpMethod=='POST'?httpParams:null);
	},
	onStateChange:function(){
		this.callback(this.req);
	}
}


//++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
//체크박스, 라디오버튼에서 선택한 항목이 있는지 검사함
//++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
function CheckRadio(Obj) {
 if (Obj.length > 1) {
 	for (var i = 0; i < Obj.length; i++) {
 		if (Obj[i].checked) return true;
 	}
 } else {
 	if (Obj.checked) return true;
 }
 return false;
}
function CheckComboRtnValue(Obj) {
	var rtn_val;
 	if (Obj.length > 1) {
 		for (var i = 0; i < Obj.length; i++) {
 			if (Obj[i].selected){
 				rtn_val=Obj[i].value;
 				break;
 			}
 		}
 	}
 	return rtn_val;
}

//++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
/* 배경 ON OFF */
//++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
var FRONT_COLOR_PRE ;
var BG_COLOR_PRE ;

function LS_PJ_bg_on ( OBJ , BG_COLOR , FRONT_COLOR ) {
	if ( OBJ.style.color ) {
		FRONT_COLOR_PRE = OBJ.style.color ;
	}
	if ( OBJ.style.backgroundColor ) {
		BG_COLOR_PRE = OBJ.style.backgroundColor ;
	}
	OBJ.style.color = FRONT_COLOR ;
	OBJ.style.backgroundColor = BG_COLOR ;
}

function LS_PJ_bg_off ( OBJ ) {
	if ( FRONT_COLOR_PRE ) {
		OBJ.style.color = FRONT_COLOR_PRE ;
	} else {
		OBJ.style.color = '#000000' ;
	}
	if ( BG_COLOR_PRE ) {
		OBJ.style.backgroundColor = BG_COLOR_PRE ;
	} else {
		OBJ.style.backgroundColor = '#FFFFFF' ;
	}
}

//++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
/* 체크박스 목록들 전체 선택/해제  CHECK ALL */
//++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
function LS_check_all( CHECKBOX_OBJ , CLASSNM ) {
	var LEN ;
	var CHECKBOX_OBJ ;
	var CLASSNM ;
	var OBJ = LS_TAG ( 'input' , CLASSNM ) ;
	if ( OBJ ) {
		LEN = OBJ.length ;
	}
	if ( LEN > 0 ) {
		for ( i=0 ; i<LEN ; i++ ) {
			if ( CHECKBOX_OBJ.checked ) {
				OBJ[i].checked = true ;
			} else {
				OBJ[i].checked = false ;
			}
		}
	} else if ( OBJ ) {
		if ( CHECKBOX_OBJ.checked ) {
			OBJ.checked = true ;
		} else {
			OBJ.checked = false ;
		}
	}
	if ( CHECKBOX_OBJ.checked ) {
		CHECKBOX_OBJ.title = 'clear all' ;
	} else {
		CHECKBOX_OBJ.title = 'select all' ;
	}
}

//GET ELEMENT BY NAME
function LS_NAME ( NM ) {
	var COLLECTIONS = document.getElementsByName ( NM ) ;
	return COLLECTIONS[0] ;
}

//GET ELEMENT BY TAG_NAME : RETURN ARRAY
function LS_TAG ( NM , CLASSNM ) {
	var COLLECTIONS = document.getElementsByTagName ( NM ) ;
	var RESULT = new Array () ;
	var j = 0 ;
	if ( CLASSNM ) {
		for ( var i=0 ; i<COLLECTIONS.length ; i++ ) {
			if ( COLLECTIONS.item(i).className == CLASSNM ) {
				RESULT[j] = COLLECTIONS.item(i) ;
				j++ ;
			}
		}
	} else {
		for ( var i=0 ; i<COLLECTIONS.length ; i++ ) {
			RESULT[i] = COLLECTIONS.item(i) ;
			j++ ;
		}
	}
	//컬렉션을 리턴하지 않는다 배열을 리턴한다
	return RESULT ;
}

//SET CLASS
function LS_set_class ( OBJ , CLASS_NAME ) {
	var OBJ ;
	var CLASS_NAME ;

	OBJ.className = CLASS_NAME ;
}

/*---------------------------------------------------------------------------------------------
 데이터 검증 함수정의 .
 ---------------------------------------------------------------------------------------------
 function name                       | description              | return
 ---------------------------------------------------------------------------------------------
 isNumeric                             숫자형데이터 검증	        0 : 오류 ,1 : 정상
 isNumber                              숫자형데이터 검증			0 : 오류 ,1 : 정상
 isHangul                              한글 데이터 검증
 checkSpace                            공백 포함검증 
 validID							   아이디 검증             
 ---------------------------------------------------------------------------------------------*/
function isNumeric(s)
{
     var isNum = /\d/;
     if( !isNum.test(s) ) {
     	alert("숫자형식이 아닙니다.");
     	//alert("Type does not match!");
     	return 0; 
     }
     return 1;
}
function isNumer(s)
{
     var isNum = /^[\d]+$/;
     if( s.search(isNum) ) {
     	alert("숫자형식이 아닙니다.");
     	//alert("Type does not match!");
     	return 0;      
     }
     return 1;
}
function isHangul(s) 
{
     var len;
     
     len = s.length;

     for (var i = 0; i < len; i++)  {
         if (s.charCodeAt(i) != 32 && (s.charCodeAt(i) < 44032 || s.charCodeAt(i) > 55203))
             return 0;
     }
     return 1;
}
function checkSpace( str )
{
     if(str.search(/\s/) != -1){
     	return 1;
     }

     else {
         return "";
     }
}
function validID( str )
{
     if( str == ""){
     	alert("아이디를 입력하세요.");   	
     	return 0;
     }     
     var retVal = checkSpace( str );              
     if( retVal != "" ) {
         alert("아이디는 빈 공간 없이 연속된 영문 소문자와 숫자만 사용할 수 있습니다.");
         return 0; 
     } 
     if( str.charAt(0) == '_') {
	 alert("아이디의 첫문자는 '_'로 시작할수 없습니다.");
	 return 0;
     }
     /* checkFormat  */
     var isID = /^[a-z0-9_]{4,12}$/;
     if( !isID.test(str) ) {
         alert("아이디는 4~12자의 영문 소문자와 숫자,특수기호(_)만 사용할 수 있습니다."); 
         return 0; 
     }
     return 1;
}
function inputCheckID(str,in_id){
	if(str!=""){
	     var retVal = checkSpace( str ); 
	     if( retVal != "" ) {
	         alert("공백을 입력할 수 없습니다.");	
	         in_id.value="";
	         return;
	     } 
	     /* checkFormat  */
	     var isID = /^[a-z0-9_]{1,20}$/;
	     if( !isID.test(str) ) {
	         alert("영문소문자와 숫자만 입력가능합니다.");	
	         in_id.value="";
	         return;
	     }
	}
}
function inputCheckName(str,in_id){
	//alert(str);
	if(str!=""){
	     var retVal = checkSpace( str ); 
	     if( retVal != "" ) {
	         alert("공백을 입력할 수 없습니다.");	
	         in_id.value="";
	         return;
	     } 	
	     /* checkFormat  */
	    if(isHangul(str)==0){ 
	         alert("한글만 입력이 가능합니다.");		
	         in_id.value="";
	         return;
	     }
	}
}
function validPWD( str )
{
     var cnt=0;
     if( str == ""){
     	alert("비밀번호를 입력하세요.");
     	return 0;
     }     
     /* check whether input value is included space or not  */
     var retVal = checkSpace( str );
     if( retVal != "") {
         alert("비밀번호에 공백이 포함될 수 없습니다.");
         return 0;
     }
     for( var i=0; i < str.length; ++i)
     {
         if( str.charAt(0) == str.substring( i, i+1 ) ) ++cnt;
     }  
     if( cnt == str.length ) {
         alert("보안상의 이유로 한 문자로 연속된 비밀번호는 허용하지 않습니다.");
         return 0; 
     }
     /* limitLength */
     //var isPW = /^[a-z0-9_~`!@\\#\$%\^&\*()-\+=\|\[\]\{\};:'"<,>.?/]{6,12}$/;
     var isPW = /^[a-zA-Z0-9]{6,20}$/;
     if( !isPW.test(str) ) {
         alert("비밀번호는 6~20자의 영문, 숫자만 사용할 수 있습니다."); 
         return 0; 
     }
     return 1;
}
function validNICKNAME( str )
{
     /* check whether input value is included space or not  */
     if( str == ""){
     	alert("닉네임을 입력하세요.");
     	return 0;
     }     
     var retVal = checkSpace( str );              
     if( retVal != "" ) {
         alert("닉네임은 2~7자의 한글, 숫자만 사용할 수 있습니다.");		
        return 0; 
     } 
     if( str.charAt(0) == '_') {
         alert("닉네임의 첫문자는 '_'로 시작할수 없습니다.");		
		return 0;
     }

     /* checkFormat  */
     var isNick = /^[가-힣0-9_]{2,6}$/;
     if( !isNick.test(str) ) {
         alert("닉네임은 2~7자의 한글, 숫자만 사용할 수 있습니다.");		
         return 0; 
     }
     return 1;
}
function validBANKPWD( str )
{
     var cnt=0;
     if( str == ""){
     	alert("보관함비밀번호를 입력하세요.");
     	return 0;
     }     
     /* check whether input value is included space or not  */
     var retVal = checkSpace( str );
     if( retVal != "") {
         alert("보관함비밀번호에 공백이 포함될 수 없습니다.");
         return 0;
     }
     /* limitLength */
     //var isPW = /^[a-z0-9_~`!@\\#\$%\^&\*()-\+=\|\[\]\{\};:'"<,>.?/]{4,12}$/;
     var isPW = /^[a-z0-9]{4,12}$/;
     if( !isPW.test(str) ) {
         alert("보관함비밀번호는 4~12자의 영문 소문자와 숫자만 사용할 수 있습니다."); 
         return 0; 
     }
     return 1;
}
function validEMAIL( str )
{
     /* check whether input value is included space or not  */
     if(str == ""){
     	alert("이메일 주소를 입력하세요.");
     	return 0;
     }
     var retVal = checkSpace( str );
     if( retVal != "") {
         alert("이메일 주소를 빈공간 없이 넣으세요.");
         return 0;
     }          
     /* checkFormat */
     var isEmail = /[-!#$%&'*+\/^_~{}|0-9a-zA-Z]+(\.[-!#$%&'*+\/^_~{}|0-9a-zA-Z]+)*@[-!#$%&'*+\/^_~{}|0-9a-zA-Z]+(\.[-!#$%&'*+\/^_~{}|0-9a-zA-Z]+)*/;
     if( !isEmail.test(str) ) {
         alert("이메일 형식이 잘못 되었습니다.");
         return 0;
     }
     if( str.length > 50 ) {
        alert("이메일 주소는 50자까지 유효합니다.");
        return 0;
     }
/*
	 if( str.lastIndexOf("daum.net") >= 0 || str.lastIndexOf("hanmail.net") >= 0 ) {
 		 alert("다음 메일 계정은 사용하실 수 없습니다.");
		 document.forms[0].email.focus();  
		 return 0;
	 }
*/
     return 1;
}
         
function validNAME(str)
{    
     if( str == '' ){
     	 alert("이름을 입력하세요");
         return 0;	
     }     
     
     var retVal = checkSpace( str );     
     if( retVal != ""){
         alert("이름은 띄어쓰기 없이 입력하세요.");
         return 0;
     }
     if( !isHangul(str) ) {
         alert("이름을 한글로 입력하세요.");
         return 0;  
     }
     if( str.length > 10 ) {
         alert("이름은 10자까지만 사용할 수 있습니다.");
         return 0;
     }
     return 1; 
}

function validENAME( str )
{
	/* check format */  
     var isENAME = /^\w/gi;

     if( !isENAME.test( str ) )
     {
         alert("영문이름을 입력하세요"); 
         document.forms[0].ename1.select();
         return 0;
     }
     return 1; 
              
}

function JuminCheck(jumin1,jumin2){
	check = false;
	total = 0;
	temp = new Array(13);

	for(i=1; i<=6; i++)
		temp[i] = jumin1.charAt(i-1);
	for(i=7; i<=13; i++)
		temp[i] = jumin2.charAt(i-7);
	
	for(i=1; i<=12; i++){
		k = i + 1;
		if(k >= 10)
			k = k % 10 + 2;
		total = total + temp[i] * k;
	}
	mm = temp[3] + temp[4];
	dd = temp[5] + temp[6];

	totalmod = total % 11;
	chd = 11 - totalmod;
	if(chd == temp[13] && mm < 13 && dd < 32 && (temp[7]==1 || temp[7]==2))
		check=true;
	return check;
}

/*---------------------------------------------------------------------------------------------
//아이프레임 리사이즈
---------------------------------------------------------------------------------------------*/
function ifrm_reSize(id,h) {
	var ifrm=document.getElementById(id);
	if(document.all){
		ifrm.style.height = h;
	}else{
		ifrm.style.height = h + "px";  
	}
}

/*---------------------------------------------------------------------------------------------
// jquery 중앙에 팝업. (레이어)
---------------------------------------------------------------------------------------------*/
function jquery_locationcenter(id) {
	var element = $('#' +  id);
	var win = $(window);     	

	var x = win.width();    
	var y = win.height();  
	
	//alert(x+"//"+y);<input type="hidden" >
	//alert(element.width()+"//"+element.height());
	
	element.css('position', 'absolute');     
	element.css('left', win.scrollLeft() + x/2 - element.width()/2);     
	//element.css('top', win.scrollTop() + y/2 - element.height()/2 + 50);  
	element.css('top', 110);  
	element.show();
	//element.css('left', mouse_x); 
	//element.css('top', mouse_y);  
};

function jquery_locationright(id) {
	var element = $('#' +  id);
	var win = $(window);     	

	var x = win.width();    
	var y = win.height();  
	
	var pos_x=x-element.width();	
	element.css('position', 'absolute');     
	element.css('left', pos_x);     
	element.css('top', 0);  
	element.show();
	//element.css('left', mouse_x); 
	//element.css('top', mouse_y);  
};


//++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
//  msec 동안 멈춤
//++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
function pause(msec)
{
	var now = new Date();
	var chkTime = now.getTime();
	var exitTime = now.getTime() + msec;
	//alert(now.getTime() + "//" + exitTime);
	var i = 1;
	while (true)
	{
		//alert(chkTime + "//" + exitTime);
			if (chkTime > exitTime)
			{
				return;
			}
		chkTime = chkTime + i;
	}
	
}

//++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
// textarea id, 제한 글자 수, 입력 결과 메세지 저장 ID
//++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
function limitCharacters(textid, limit, limitid){
	// 입력 값 저장
	var text = $('#'+textid).val();
	// 입력값 길이 저장
	var textlength = text.length;
	if(textlength > limit){
		//$('#' + limitid).html('글내용을 '+limit+'자 이상 쓸수 없습니다!');
		try{
			//uf_MakeAlertBox("SystemAlert",1,"안내","글내용을 "+limit+"자 이상 쓸수 없습니다!","","","./images/popup/title_noti.gif");	
			alert("글내용을 "+limit+"자 이상 쓸수 없습니다!");
		}catch(e){
			//parent.uf_MakeAlertBox("SystemAlert",1,"안내","글내용을 "+limit+"자 이상 쓸수 없습니다!","","","./images/popup/title_noti.gif");		
			alert("글내용을 "+limit+"자 이상 쓸수 없습니다!");
		}
		// 제한 글자 길이만큼 값 재 저장
		$('#'+textid).val(text.substr(0,limit));
		return false;
	}else{
		$('#' + limitid).html((limit - textlength) +' 자 남음');
		return true;
	}
}

//$(function(){
//	$('#contents').keyup(function(){
//		limitCharacters('contents', 20, 'charlimitid');
//	})
//});



function uf_getNavigator(){
	var gNavi=""
	if (navigator.userAgent.indexOf("MSIE 6") > 0) gNavi = "IE6";        // IE 6.x
	else if(navigator.userAgent.indexOf("MSIE 7") > 0) gNavi = "IE7";    // IE 7.x
	else if(navigator.userAgent.indexOf("MSIE 8") > 0) gNavi = "IE8";    // IE 8.x
	else if(navigator.userAgent.indexOf("MSIE 9") > 0) gNavi = "IE9";    // IE 9.x
	else if(navigator.userAgent.indexOf("Firefox") > 0) gNavi = "FF";   // FF
	else if(navigator.userAgent.indexOf("Opera") > 0) gNavi = "OR";     // Opera
	else if(navigator.userAgent.indexOf("Chrome") > 0) gNavi = "CM";    // Chrome
	else if(navigator.userAgent.indexOf("Netscape") > 0) gNavi = "NS";  // Netscape
	return gNavi;
}

function uf_PrintCurrTime() {
	var clock = document.getElementById("l_curr_clock"); // 출력할 장소 선택
	var now = new Date(); // 현재시간
	var now_year = now.getFullYear();
	var now_month = now.getMonth()+1;
	var now_day = now.getDate();
	var now_hour = now.getHours();
	var now_minute = now.getMinutes();
	var now_second = now.getSeconds();
	
	var nowTime = uf_leadingZeros(now_year,4) + "." + uf_leadingZeros(now_month,2) + "." + uf_leadingZeros(now_day,2) + " " + uf_leadingZeros(now_hour,2) + ":" + uf_leadingZeros(now_minute,2) + ":" + uf_leadingZeros(now_second,2);
	clock.innerHTML = nowTime; // 현재시간을 출력
	setTimeout("uf_PrintCurrTime()",1000); // setTimeout(“실행할함수”,시간) 시간은1초의 경우 1000
}

function uf_leadingZeros(n, digits) {
  var zero = '';
  n = n.toString();

  if (n.length < digits) {
    for (i = 0; i < digits - n.length; i++)
      zero += '0';
  }
  return zero + n;
}

//----------------------------------------------------------------------------------------------
//		콤마넣기(문자열)
//----------------------------------------------------------------------------------------------
function setComma(str) {	
	str = getNumber(str.toString());
	var regx = new RegExp(/(-?\d+)(\d{3})/);
	var bExists = str.indexOf(".",0);
	var strArr = str.split('.');
	var retStr = '';

	while(regx.test(strArr[0])) {
		strArr[0] = strArr[0].replace(regx,"$1,$2");
	}

	if (bExists > -1) {
		retStr = strArr[0] + "." + strArr[1];
	} else {
		retStr = strArr[0];
	}
	return retStr;
}

//----------------------------------------------------------------------------------------------
//		콤마제거
//----------------------------------------------------------------------------------------------
function getNumber(str) {
	if (str == undefined){
		return '';
	}
	str = "" + str.replace(/,/gi,''); // 콤마 제거
	str = str.replace(/(^\s*)|(\s*$)/g, ""); // trim
	//str = str.replace(/^0+/g, ""); // remove receding zeros
	return str;
}

//----------------------------------------------------------------------------------------------
//쿠키굽기
//----------------------------------------------------------------------------------------------
function setCookie( name, value, expiredays ){ 
	var todayDate = new Date(); 
	todayDate.setDate( todayDate.getDate() + expiredays ); 
	document.cookie = name + "=" + escape( value ) + "; path=/; expires=" + todayDate.toGMTString() + ";" 
} 
//쿠키시간별로 굽기
function setCookieHour(name, value, expirehour){
	var todayDate = new Date(); 
	todayDate.setTime(todayDate.getTime() + expirehour ); 
	document.cookie = name + "=" + escape( value ) + "; path=/; expires=" + todayDate.toGMTString() + ";" 
}
//쿠키읽기
function getCookie( name ){
	var nameOfCookie = name + "=";
	var x = 0;
	while ( x <= document.cookie.length ){
		var y = (x+nameOfCookie.length);
		if ( document.cookie.substring( x, y ) == nameOfCookie ) {
			if ( (endOfCookie=document.cookie.indexOf( ";", y )) == -1 )
			endOfCookie = document.cookie.length;
			return unescape( document.cookie.substring( y, endOfCookie ) );
		}
		x = document.cookie.indexOf( " ", x ) + 1;
		if ( x == 0 )
		break;
	}
	return "";
}


//----------------------------------------------------------------------------------------------
// 사이트 인터페이스
//----------------------------------------------------------------------------------------------
//로그인폼서밋
function check_login_form(){
	var frm=document.login_form;
	frm.target="HiddenFrame";
	if(frm.u_id.value==""){
		alert("아이디를 입력하세요.");
		frm.u_id.focus();
		return false;
	}	
	if(frm.u_pw.value==""){
		alert("비밀번호를 입력하세요.");
		frm.u_pw.focus();
		return false;
	}
}

//게임실행
function gamestart(num){
	if(CheckActiveX()==0){
		alert("엑티브엑스를 설치하셔야 게임실행이 가능합니다\n엑티브엑스는 인터넷 익스플로러에서만 설치가 가능합니다.");	
	}else{
		document.getElementById('HiddenFrame').contentWindow.location='/game/gamestart.asp?gameno='+ num;
	}
}

//엑티브엑스 설치여부
function CheckActiveX(){	
	var ret=0;
	if (document.getElementById("gameLoader").object!=null){
		ret=1;
	}else{
		ret=0;
	}
  return ret;
}

/*---------------------------------------------------------------------------------
// 사이트메뉴 공통주소 이동.
----------------------------------------------------------------------------------*/
function uf_CommonUrl(g_target, g_menu){
	var url="";
	var p_w=0;
	var p_h=0;
	
	switch (g_menu) {
    case "main":
			url="/";
			break;
		case "customer":
			url="/customer/notice_list.html";
			break;		
		case "mypage":
			url="/member/myinfo.html";
			break;    
    case "join":
			url="/member/join01.html";
			break;
    case "idpwfind":
			url="#";
			break;		
		case "avatar":
			url="/game/avatarmall.html";
			break;
    case "paycard":
			url="/member/paycard.html";
			p_w=500;
			p_h=700;
			break;
    case "saved_money":
			url="/charge/saved_money_change.html";
			break;
		case "cashcharge":
			alert("결제서비스는 서비스 준비중 입니다.\n이용에 불편을 드려 대단히 죄송합니다");
			g_target=0;
			url="/charge/cashcharge";
			p_w=500;
			p_h=500;
			break;
    case "freecharge":
			url="/charge/freecharge.asp";
			break;
    case "download":
			url="/game/download.html";
			break;
    case "qna":
			url="/member/qna.html";
			break;
    case "login":
			url="/member/login.html";
			break;
		case "logout":
			url="/member/logout.html";
			break;
    case "bank":
			url="/member/gold_account.html";
			g_target="1";
			p_w=417;
			p_h=540;
			break;
    case "matgo":
			url="/game/game02.html";
			break;
    case "badugi":
			url="/game/game01.html";
			break;
    case "memout":
			url="/member/mem_out.html";
			break;	
		case "sitemap":
			url="#"
			break;
	} 	
	//타겟에 따른이동 (1:페이지이동, 2:팝업, 3:숨김프레임)	
	if(g_target==1){document.location.href=url;}
	if(g_target==2){pop_win('popup', url, 0, 0, p_w, p_h, 0, 0, 0, 0, 0);}
	if(g_target==3){document.getElementById('HiddenFrame').contentWindow.location=url;}
}

//아이프레임콘텐츠사이즈 수정
function resize_frame(id) {
	var frm = document.getElementById(id);
	function resize() {
		frm.style.height = "auto"; // set default height for Opera
		contentHeight = frm.contentWindow.document.documentElement.scrollHeight;
		frm.style.height = contentHeight + 30 + "px"; // 23px for IE7
	}
	if(frm.addEventListener){
		frm.addEventListener('load', resize, false);
	}else{
		frm.attachEvent('onload', resize);
	}
}
// 상단 검색메뉴
function keyword_search(){
	var frm=document.form_search;
	frm.action="/search/search.asp";
	if(frm.keyword.value==""){return false;}
}

//메모,친구등록 팝업 오픈
function uf_MemoFriendsOpen(f_no,e) {
	var divTop = e.clientY-20;
	var divLeft = e.clientX-30;
	
	var innerHtml="";
	innerHtml="<div id='divMemoFriends' style='display:none;border:1px solid #ff0000;z-index:9999;' onmouseout='uf_MemoFriendsClose();' onmouseover='uf_MemoFriendsShow();'>";
	innerHtml=innerHtml+"<a href='#' onclick='uf_SendMemoPop("+f_no+");'><img src='/images/l_pop_memo.gif' alt='' /></a><br/>";
	innerHtml=innerHtml+"<a href='#' onclick='uf_SendFriendPop("+f_no+");'><img src='/images/l_pop_friends.gif' alt='' /></a>";
	innerHtml=innerHtml+"</div>";
	$('body').append(innerHtml);
	$('#divMemoFriends').css({"top": divTop,"left": divLeft, "position": "absolute"}).show();
}
//메모,친구등록 팝업 닫기
function uf_MemoFriendsClose(){
	$('#divMemoFriends').hide();	
}
function uf_MemoFriendsShow(){
	$('#divMemoFriends').show();	
}
//메모팝업
function uf_SendMemoPop(f_no){
	uf_MemoFriendsClose();
	pop_win("memo_send", "/mypage/memo_send.asp", 0, 0, 417, 402, 0, 0, 0, 0, 0);
}
//친구팝업
function uf_SendFriendPop(f_no){
	uf_MemoFriendsClose();
	pop_win("friends_send", "/mypage/friends_send.asp", 0, 0, 417, 402, 0, 0, 0, 0, 0);
}


//로그인폼서밋
function uf_LoginSubmit(frm) {
    /*
	frm.action="/member/login_proc.asp";
	frm.target="HiddenFrame";
	if(frm.m_id.value==""){
		alert("아이디를 입력하세요!");
		frm.m_id.focus();
		return false;	
	}
	if(frm.m_pw.value==""){
		alert("비밀번호를 입력하세요!");
		frm.m_pw.focus();
		return false;	
	}*/

    $.ajax({
        url: "/member/LoginProc",
        data: {
            "m_id": $("#LoginID").val(),
            "m_pw": $("#LoginPWD").val(),
            "m_save": ($("#SaveIDCheck").is(":checked") ? "Y" : "N")
        },
        dataType: "json",
        method: "POST",
        success: function (res) {
            alert(res);
        }, error: function (x) {
            alert("서버 오류로 인해 로그인을 할 수 없습니다");
            return false;
        }
    });
}	
//이미지 가로세로, 리사이즈
function uf_ImageResize(img, w, h) {
  var max_width = w, //가로 최대사이즈
      max_height = h; //세로 최대사이즈      
  var width = $("#"+img).outerWidth(); //현재 가로값
  var height = $("#"+img).outerHeight(); //현재 세로값
  var en = width - max_width < height - max_width ? 
      max_width / width :
      max_height / height;
  var mod_width = width * en; //이미지 줄인사이즈 가로
  var mod_height = height * en; //이미지 줄인사이즈 세로  
  //alert(mod_width);    
  //alert(mod_height);
  $("#"+img).css({width:mod_width, height:mod_height}); //현재 이미지의 사이즈 재정의      
}
//이미지넓이 리사이즈
function uf_ImageWidthResize(img, w) {
  var maxWidth = w; // Max width for the image
  var ratio = 0;  // Used for aspect ratio
  var width = $("#"+img).width();    // Current image width
  var height = $("#"+img).height();  // Current image height

  // Check if the current width is larger than the max
  if(width > maxWidth){
    ratio = maxWidth / width;   // get ratio for scaling image
    $("#"+img).css("width", maxWidth); // Set new width
    $("#"+img).css("height", height * ratio);  // Scale height based on ratio
    height = height * ratio;    // Reset height to match scaled image
    width = width * ratio;    // Reset width to match scaled image
  }
}

/*
1. checked 여부 확인
$("input:checkbox[id='ID']").is(":checked") == true : false
  $("input:checkbox[name='NAME']").is(":checked") == true : false
2. checked/unchecked 처리
$("input:checkbox[id='ID']").attr("checked", true);
$("input:checkbox[name='NAME']").attr("checked", false);

3. 특정 라디오버튼 선택 / 모든 라디오버튼 선택해제
$("input:radio[name='NAME']:radio[value='VALUE']").attr("checked",true);
$("input:radio[name='NAME']").removeAttr("checked");
*/


function uf_Right(str, n){
    if (n <= 0)
        return "";
    else if (n > String(str).length)
        return str;
    else {
        var iLen = String(str).length;
        return String(str).substring(iLen, iLen - n);
    }
}