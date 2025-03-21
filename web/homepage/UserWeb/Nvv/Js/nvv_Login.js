//用户登录
function fnLoginOk() {

	var sLoginId = $("#LoginID").val();
	var sLoginPwd = $("#LoginPWD").val();
	//var sLoginCode = $("#fmLoginCode").val();
	//账号不能为空值
	if (sLoginId === "") {
		layer.msg('아이디를 입력하세요1.', { shade: 0.8 });
		$("#fmLoginId").focus();
		return false;
	}
	//密码长度不符合规范
	if (sLoginId.length < 2 || sLoginId.length > 12) {
		layer.msg("아이디를 정확히 입력하세요.", { shade: 0.8 });
		$("#fmLoginId").focus();
		return false;
	}
	//密码不能为空值
	if (sLoginPwd === "") {
		layer.msg("비밀번호를 입력하세요.", { shade: 0.8 });
		$("#fmLoginPwd").focus();
		return false;
	}
	//密码长度不符合规范
	if (sLoginPwd.length < 6 || sLoginPwd.length > 12) {
		layer.msg("비밀번호를 정확히 입력하세요.", { shade: 0.8 });
		$("#fmLoginPwd").focus();
		return false;
	}
	//登陆验证码不能为空值
/*	if (sLoginCode == ""){
		layer.msg("인증코드를 입력 하세요.", { shade: 0.8 });
		$("#fmLoginCode").focus();
		return false;
	}
	//验证码长度为4位
	if (sLoginCode.length != 4) {
		layer.msg("인증번호는 숫자 4자리만 입력할 수 있습니다.", { shade: 0.8 });
		$("#fmLoginCode").focus();
		return false;
	}
*/
	$.ajax({
		type: "POST",
		url: "/Main/PerformLogin",
		dataType: "json",
		data: {
			sLoginId: sLoginId,
			sLoginPwd: sLoginPwd,
		//	sLoginCode:sLoginCode
            "ReturnURL": $("#ReturnURL").val()
		},
		timeout: 15000,
		success: function (Data) {
			if (Data.nRtnCode === 200) {
				window.location.href = "/Login/Index";
			} else {
				layer.msg(Data.sError, { shade: 0.8 });
				if (Data.nRtnCode === 1006 || Data.nRtnCode === 1007) {
					fnLoginCode();
				}
			}
		}
	});
}

//Enter键
function fnLoginBoxEnter() {
	if (event.keyCode === 13) {
		event.returnValue = false;
		event.cancel = true;
		fnLoginOk();
	}
}

//刷新验证码
function fnLoginCode() {
	//a.src = a.src + '?';
	document.getElementById("imgCode").src = document.getElementById("imgCode").src + '?';
	$("#fmLoginCode").val("");
	$("#fmLoginCode").focus();
}