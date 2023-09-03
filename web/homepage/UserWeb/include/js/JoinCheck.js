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
     var isID = /^[a-z0-9_]{4,20}$/;
     if( !isID.test(str) ) {
         alert("아이디는 4~20자의 영문 소문자와 숫자,특수기호(_)만 사용할 수 있습니다."); 
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
	}else{
		alert("이름을 입력하세요");
		in_id.value="";
 		return;
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
     	alert("닉네임은 2~12자의 공백없이 한글과 숫자,특수기호(_)만 사용할 수 있습니다.");		
        return 0; 
     } 
     if( str.charAt(0) == '_') {
     	alert("닉네임의 첫문자는 '_'로 시작할수 없습니다.");		
		return 0;
     }

     /* checkFormat  */
     var isNick = /^[가-힣0-9_]{2,12}$/;
     if( !isNick.test(str) ) {
         alert("닉네임은 2~12자의 공백없이 한글과 숫자,특수기호(_)만 사용할 수 있습니다.");		
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
