$(document).ready(function() {
	// 페이징
	pagingOver();
	// gnb
	gnbEvent();
});

//++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
//페이지내 포커스없애기
//++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
function autoBlur(){ 
	if(event.srcElement.tagName=="A"||event.srcElement.tagName=="/img"||event.srcElement.tagName=="/button"){
		document.body.focus(); 
	}
}
document.onfocusin=autoBlur; 


function btnOver() {
	$(".btn_hover").hover(
		function() {
			$(this).attr("src", $(this).attr("src").replace(".png", "_over.png"));
		}, function() {
			$(this).attr("src", $(this).attr("src").replace("_over.png", ".png"));
		}
	)
}

// 페이징
function pagingOver() {
	var n_index = 0;
	$(".paging em a").mouseover(function() {
			var i = $(".paging em a").index($(this));
			$(this).parent("em").addClass("type_"+i);
			n_idex = i;
	});
	$(".paging em a").mouseout(function() {
		$(this).parent("em").removeClass("type_"+n_idex);
	});
}

function gnbEvent() {
	$("#gnb").mouseover(function() {
			$("#gnb").addClass("on");
			$("#bg_gnb").show();
	});
	$("#gnb").mouseout(function() {
		$("#gnb").removeClass("on");
		$("#bg_gnb").hide();
	});
}


function inputLabelShow(obj) {
	var sub_item = $(obj).children();
	$(sub_item).on('click', function () {
		$(this).parent().find("label").hide();
		$(this).parent().find("input").focus();
	});
	$(sub_item).on('blur', function () {
		if ($.trim(jQuery(this).val()).length === 0) {
			$(this).siblings('label').show();
		}
	});
}



var MainBaner = {
	timer: 0,
	box: $("#main_notice"),
	items: [],
	nCnt: 0,
	total: 0,
	init: function() {
		this.total =  $("#main_notice .n_list ul li").size();
		this.items = $("#main_notice .n_list ul li"); 
		
		$(this.items).eq(0).addClass("active");
		if(this.total > 1){
			this.loop();
		
			$("#main_notice .i_prev").click(function() {
	    		clearInterval(MainBaner.timer);
	    		var nIdx = (MainBaner.nCnt == 0) ? MainBaner.total-1 : MainBaner.nCnt-1; 
	   			MainBaner.prev(MainBaner.nCnt, nIdx);
	    		MainBaner.loop();
	    		MainBaner.nCnt = nIdx;
			});
			$("#main_notice .i_next").click(function() {
				clearInterval(MainBaner.timer);
	    		var nIdx = (MainBaner.nCnt == MainBaner.total-1) ? 0 : MainBaner.nCnt+1; 
	   			MainBaner.next(MainBaner.nCnt, nIdx);
	    		MainBaner.loop();
	    		MainBaner.nCnt = nIdx;
			});

		}
	}, 
	loop: function() {
		MainBaner.timer = setInterval(function () {
			MainBaner.start();
		}, 3000);
	},
	start: function() {
		if(this.nCnt < this.total-1) {
			this.next(this.nCnt, this.nCnt+1);
			this.nCnt++;
    	} else if(this.nCnt == this.total-1) {
    		this.next(this.nCnt, 0);
    		this.nCnt = 0;
    	}
	},
	next: function(oldItem, newItem) {
		
		$(this.items).eq(newItem).addClass("active");
		$(this.items).eq(newItem).css({"top" : "-30px"});
		
		$(this.items).eq(oldItem).animate({
			"top" : "30px"
		}, 300, function() {
			$(this).removeClass("active");
			$(this).css({"top" : 0});
		});
		
		$(this.items).eq(newItem).animate({"top" : "0"});
		
	}, 
	prev: function(oldItem, newItem) {
		$(this.items).eq(newItem).addClass("active");
		$(this.items).eq(newItem).css({"top" : "30px"});
		
		$(this.items).eq(oldItem).animate({
			"top" : "-30px"
		}, 300, function() {
			$(this).removeClass("active");
			$(this).css({"top" : 0});
		});
		
		$(this.items).eq(newItem).animate({"top" : "0"});
	}
};

var joy = joy || {};
joy.FSFZBanner= {
		container: $("#main_banner"),
		timer:0,
		total: 0,
	    nCnt: 0,
	    auto: "false",
	    init: function () {
	    	 this.total =  $("#main_banner .i_list li").size();
	    	 var _this = this;
	    	 
	    	 if(this.auto == true && this.total > 1) {
		    		this.loop();
		    	}
	    	
	    	//$("#head_area").append('<div class="item_nav"></div>');
	    	$("#main_banner .item_txt").append('<ul></ul>');
	    	for(var i=0; i<this.total; i++) {
	    		$("#main_banner .item_txt ul").append("<li></li>");
	    		$("#head_area .item_nav").append('<em class="idx_' + i +  '"></em>')
	    	}
	    	$("#main_banner .item_txt li").eq(0).addClass("active");
	    	$("#head_area .item_nav em").eq(0).addClass("active");
	    	
	    	$("#main_banner .item_txt li").each(function(i, item) {
                $(item).html($("#main_banner .i_list li").eq(i).find(".i_txt").html());

	    	});
	    	
	    	$("#head_area .item_nav em").click(function(e) {
	    		var element = e.target;
	    		var idx = $("#head_area .item_nav em").index($(element));
	    		if(idx != joy.FSFZBanner.nCnt) {
		    		clearInterval(joy.FSFZBanner.timer);
		    		joy.FSFZBanner.next(joy.FSFZBanner.nCnt, idx);
		    		joy.FSFZBanner.nCnt= idx;
		    		joy.FSFZBanner.loop();
	    		}
	    	});
	    },  
	    loop: function() {
	    	joy.FSFZBanner.timer = setInterval(function () {
	    		joy.FSFZBanner.start();
    		}, 8000);
	    },
	    
	    start: function() {
	    	if(this.nCnt < this.total-1) {
	    		this.next(this.nCnt, this.nCnt+1);
	    		this.nCnt++;
	    	} else if(this.nCnt == this.total-1) {
	    		this.next(this.nCnt, 0);
	    		this.nCnt = 0;
	    	}
	    },
	    next: function(oldItem, newItem) {
	    	$("#head_area .item_nav em").removeClass("active");
			$("#head_area .item_nav em").eq(newItem).addClass("active");
			
    		$("#main_banner .item_txt li").eq(newItem).css({
    			"marginTop" : "-132px"
    		}, 300, function() {});
	    	$("#main_banner .item_txt li").eq(oldItem).animate({
                top: "132px"
	    	}, 300, function() {
	    		$("#main_banner .item_txt li").eq(newItem).animate({
		    		"marginTop": "0"
		    	}, 300, function() {
		    		
		    	});
	    		
				$("#main_banner .item_txt li").removeClass("active");
                $("#main_banner .item_txt li").eq(newItem).addClass("active");
	    		$("#main_banner .item_txt li").eq(oldItem).css({
	    			  "top": 0
	    		  })
	    	});
	    	
	    	$("#main_banner .i_list li").eq(newItem).show();
			$("#main_banner .i_list li").eq(oldItem).animate({
	    	    opacity: 0.3,
	    	    //left: "-280px",
	    	    top:"400px"
	    	  }, 400, function() {
	    		  $("#main_banner .i_list li").eq(oldItem).hide();
                  $("#main_banner .i_list li").removeClass("active");
                  $("#main_banner .i_list li").eq(newItem).addClass("active");
                  $("#main_banner .i_list li").eq(oldItem).css({
                      "filter" : "alpha(opacity = 100)",
	    			  "opacity" : 1,
	    			  "left" : 0,
	    			  "top": 0,
	    			  "display":"none"
	    		  })
	    	  });
	    }	    
}


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
			p_w=600;
			p_h=700;
			break;
    case "saved_money":
			url="/charge/saved_money_change.html";
			break;
		case "cashcharge":
			alert("결제서비스는 서비스 준비중 입니다.\n이용에 불편을 드려 대단히 죄송합니다");
			g_target=0;
			url="/charge/cashcharge";
			p_w=550;
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
            url = "/member/mem_out.html";
            break;
        case "SelfRestricted":
            url = "/member/SelfRestricted.html";
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

//++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
//동적 플래쉬파일실행
//++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
function FlashFilePlay(filename,w,h){
document.write("<object classid='clsid:D27CDB6E-AE6D-11cf-96B8-444553540000' codebase='https://active.macromedia.com/flash4/cabs/swflash.cab#version=4,0,0,0' width='"+w+"' height='"+h+"'>");
document.write("<param name='movie' value='"+filename+"'>");
document.write("<param name='wmode' Value='Transparent'>");
document.write("<param name='play' value='true'>");
document.write("<param name='loop' value='true'>");
document.write("<param name='quality' value='high'>");
document.write("<embed src='"+filename+"' play='true' loop='true' quality='high' pluginspage='https://www.macromedia.com/shockwave/download/index.cgi?P1_Prod_Version=ShockwaveFlash' width='"+w+"' height='"+h+"' wmode='Transparent'></embed>");
document.write("</object>");
}

/*---------------------------------------------------------------------------------------------
// jquery 중앙에 팝업. (레이어)
---------------------------------------------------------------------------------------------*/
function jquery_layerpop(id, x_pos) {
	var element = $('#' +  id);
	var win = $(window);     	

	var x = win.width();    
	var y = win.height();  
	
	//alert(x+"//"+y);<input type="hidden" >
	//alert(element.width()+"//"+element.height());
	//element.css('top', win.scrollTop() + y/2 - element.height()/2 + 50);  
	
	element.css('position', 'absolute');     
	
	var pos_x;
	var pos_y=element.css('top', win.scrollTop() + y/2 - element.height()/2 + 50);  ;
	if(x_pos=='left'){
		element.css('left', 10);     		
		element.css('top', pos_y);  
	}
	if(x_pos=='right'){
		pos_x=x-element.width();	
		element.css('left', pos_x);     
		element.css('top', pos_y);  
	}
	if(x_pos=='center'){
		element.css('left', win.scrollLeft() + x/2 - element.width()/2);     
		element.css('top', pos_y);  
	}	
	element.show();
	//element.css('left', mouse_x); 
	//element.css('top', mouse_y);  
};

// 체크 되어 있는 값 추출
function uf_getCheckedVal(id){
	var checked_val="";
	$("input[id="+id+"]:checked").each(function() {
		if(checked_val==""){
		  checked_val=$(this).val();
		}else{
			checked_val+=','+$(this).val();	
		}
	});
	return checked_val;
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


var popup_zindex=1000;

/* 풀스크린 레이어 만들기 */
function uf_fullScreenMake(id){
	var $layer = $('#'+id);
	var maskHeight = $(document).height(); 
	var maskWidth = $(window).width(); 	
	$layer.css({'height':maskHeight,'width':maskWidth,'left':0,'top':0});
	$layer.fadeTo('fast',0.4); 
	popup_zindex+=1;$layer.css("z-index",popup_zindex);
	$layer.show();
}

/* 윈도우 리사이즈에 따른 마스크 수정*/
function uf_MaskResize(){
	var maskHeight = $(document).height(); 
	var maskWidth = $(window).width(); 	
	var $layer = $('#fullscreen_mask');
	$layer.css({'height':maskHeight,'width':maskWidth,'left':0,'top':0});
}

/* mask close + quickview close*/
function uf_maskClose(){
	$('.popup_frames').hide();
}

/* maske click == close all*/
$('#fullscreen_mask').click(function(){uf_maskClose();}); 

//윈도우 크기 변경 이벤트가 발생하면  
$(window).resize(function(){ 
	uf_MaskResize();
});  

/* ESC EVENT*/
$(document).keyup(function(e) { 
	//console.log(e.which);
  if (e.which === 13){}  // enter $('.save').click();
  if (e.which === 27){uf_maskClose();}  // esc   $('.cancel').click();
});


/* VOD 팝업오픈 */
function uf_MainVodOpen(){
	uf_fullScreenMake('fullscreen_mask');
	var $layer = $('#popup_vod');
	uf_LayerCenter('popup_vod');
	popup_zindex+=1;$layer.css("z-index",popup_zindex);
	$layer.show();		
}


/* layer center popup*/
function uf_LayerCenter(lid){	
	var $layer = $('#'+lid);
	var left = ( $(window).scrollLeft() + ($(window).width() - $layer.width()) / 2 );
	var top = (($(window).height() - $layer.height()) / 2 );
	$layer.css({'left':left,'top':top,'position':'fixed'});	
}	