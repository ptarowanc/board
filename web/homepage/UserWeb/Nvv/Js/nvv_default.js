$(function(){
	$(".xjALertClose button").click(function(){
		$(".xjAlertGame").css("display","none")
	})	
})

function fnAlertShow(){
	$(".xjAlertGame").css("display","block")
}

function fnGameContent(a) {
	switch (a) {
	case 1:
		$(".xjGamecongtentA").removeClass("hidden")
		$(".xjGamecongtentB").addClass("hidden")
		$(".xjGamecongtentC").addClass("hidden")
		break;
	case 2:
		$(".xjGamecongtentA").addClass("hidden")
		$(".xjGamecongtentB").removeClass("hidden")
		$(".xjGamecongtentC").addClass("hidden")
		break;
	case 3:
		$(".xjGamecongtentA").addClass("hidden")
		$(".xjGamecongtentB").addClass("hidden")
		$(".xjGamecongtentC").removeClass("hidden")
		break;
	}
}

function fnCloseModelWindow() {
	$("button").blur()
	if ($(".alertError").hasClass("hidden") == false) {
		if ($(".changePwd").hasClass("hidden") == true) {
			$(".alertError").addClass("hidden")
			$(".modalBg").addClass("hidden")
			return false
		}
		$(".alertError").addClass("hidden")
		return false
	}
	$(".xjmodal").addClass("hidden")
	$(".modalBg").addClass("hidden")
}

function fnOpenModal(sError) {
	$("#ajaxErrorInfo").empty();
	$("#ajaxErrorInfo").append(sError);
	$(".alertError").removeClass("hidden");
	$(".modalBg").removeClass("hidden");
	setTimeout("fnCloseModelWindow()", 2000);
}

function fnFootPopDisplay(nType) {
	switch (nType) {
		case 1:
			$("#MainBottomFontAlert1").removeClass("hidden");
			$(".modalBg").removeClass("hidden");
			break;
		case 2:
			$("#MainBottomFontAlert2").removeClass("hidden");
			$(".modalBg").removeClass("hidden");
			break;
		case 3:
			$("#MainBottomFontAlert3").removeClass("hidden");
			$(".modalBg").removeClass("hidden");
			break;
		case 4:
			$("#MainBottomFontAlert4").removeClass("hidden");
			$(".modalBg").removeClass("hidden");
			break;
		default:
			$("#MainBottomFontAlert1").removeClass("hidden");
			$(".modalBg").removeClass("hidden");
			break;
	}
}


$(function () {
	$(document).on("click", ".btnmodalClose", function () {
		$(this).parents(".xjmodal").addClass("hidden")
		if ($(".changePwd").hasClass("hidden") == false) {
			return false
		}

		$(".modalBg").addClass("hidden")
	})

	$(document).on("click", ".mainNoticeListBg li:not('.noneData'),.subNoticeList li:not('.noneData')", function () {
		$(".noticeView").removeClass("hidden")
		$(".modalBg").removeClass("hidden")
	})

	$(document).on("click", ".gameNav", function () {
		var a = $(this).parent().index();
		$(".guideimgView>div").addClass("hidden")
		$(".guideimgView>div").eq(a).removeClass("hidden")
	})

	//$(document).on("click", ".mainitemList ul li:last-child img,.subItemList ul li:last-child img", function () {
	//	$(".itemBuyModal").removeClass("hidden")
	//	$(".modalBg").removeClass("hidden")
	//})

	//$(document).on("click", ".freeCashImg", function () {
	//	$(".cashFreeModal").removeClass("hidden")
	//	$(".modalBg").removeClass("hidden")
	//})

	$(document).on("click", ".modalBg", function () {
		$("button").blur()
		if ($(".alertError").hasClass("hidden") == false) {
			if ($(".changePwd").hasClass("hidden") == true) {
				console.log(2)
				$(".alertError").addClass("hidden")
				$(".modalBg").addClass("hidden")
				return false
			}
			$(".alertError").addClass("hidden")
			return false
		}
		$(".xjmodal").addClass("hidden")
		$(".modalBg").addClass("hidden")
	})

	$(document).on("click", ".mainNoticeListBtn,.subNoticeListBtn img", function () {
		$(".noticeList").removeClass("hidden")
		$(".modalBg").removeClass("hidden")
	})


	$(document).on("click", ".noticeListContent table a", function () {
		$(".noticeList").addClass("hidden")
		$(".noticeView").removeClass("hidden")
	})


	$(document).on("click", ".returnNoticeList", function () {
		$(".noticeList").removeClass("hidden")
		$(".noticeView").addClass("hidden")
	})


	$(document).on("click", ".itemBuyBtn", function () {
		$(".buySuccess").removeClass("hidden")
		$(".itemBuyModal").addClass("hidden")
	})

	//$(document).on("click", ".loginSearch", function () {
	//	$(".searchModalId").removeClass("hidden")
	//	$(".modalBg").removeClass("hidden")
	//})




	//	$(document).on("click",".searchModalId #fmSearchId",function(){
	//		$(".searchModalPwd #fmSearchId").click()
	//		$(".searchModalId").removeClass("hidden")
	//		$(".searchModalPwd").addClass("hidden")
	//	})

	//	$(document).on("click",".searchModalPwd #fmSearchPwd",function(){
	//		$(".searchModalId").addClass("hidden")
	//		$(".searchModalPwd").removeClass("hidden")
	//		$(".searchModalPwd #fmSearchPwd").click()
	//	})

	//$(document).on("click", ".mainAlertBtn img", function () {
	//	$(".modalBg").removeClass("hidden")
	//	$(".MainBottomFontAlert").removeClass("hidden")
	//})

	//$(document).on("click", ".changepwdBtn", function () {
	//	$(".modalBg").removeClass("hidden")
	//	$(".changePwd").removeClass("hidden")
	//})


	$(document).on("click", ".guideBtn img", function () {
		var a = $(this).parent().parent().index();

		switch (a) {
			case 0:
				$(".gameAlertView img").attr("src", "/img/guide/tree_b.png")
				break;
			case 1:
				$(".gameAlertView img").attr("src", "/img/guide/tree_p.png")
				break;
			case 2:
				$(".gameAlertView img").attr("src", "/img/guide/tree_m.png")
				break;
		}

		$(".modalBg").removeClass("hidden")
		$(".gameAlert").removeClass("hidden")
	})

	$(document).on("click", ".erroeClose", function () {
		$(".alertError").addClass("hidden")
		if ($(".changePwd").hasClass("hidden") == false) {
			return false
		}
		$(".modalBg").addClass("hidden")
	})

	$(document).on("click", ".mainFootAlertImg span", function () {
		$(".modalBg").removeClass("hidden")
		$(".MainBottomImgAlert").removeClass("hidden")
	})

	$(document).on("click", ".changePwdAlertBtn", function () {
		$(".alertError").removeClass("hidden")
	})



	$(document).keydown(function (event) {
		if (event.keyCode == 13) {
			$(".modalBg").click()
		}
		if (event.keyCode == 27) {
			$(".modalBg").click()
		}
	});

})


function fnMemberAlertLeft() {
	$(".memberjoinIpinLeft").removeClass("hidden")
	//$(".modalBg").removeClass("hidden")
}

function fnMemberAlertRight() {
	$(".memberjoinIpinRight").removeClass("hidden")
	//$(".modalBg").removeClass("hidden")
}

//$(function () {
//    var body = $('.mainBg');
//    //var bg = $('.mainBg').style.backgroundImage;
//    var backgrounds = [
//        'url(/img/bg2.jpg)',
//        'url(/img/bg1.jpg)',
//        'url(/img/bg3.jpg)',
//        'url(/img/bg4.jpg)'];
//        //'/img/bg2.jpg',
//        //'/img/bg1.jpg',
//        //'/img/bg3.jpg',
//        //'/img/bg4.jpg'];
//    var current = 0;

//    function nextBackground() {
//        //bg.url = backgrounds[current = ++current % backgrounds.length];
//        body.css(
//            'backgroundImage',
//            //'background',
//            backgrounds[current = ++current % backgrounds.length]);

//        setTimeout(nextBackground, 15000);
//    }
//    setTimeout(nextBackground, 15000);
//    body.css('backgroundImage', backgrounds[0]);
//});