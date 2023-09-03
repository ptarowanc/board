

/**
 * lcslog.js  v 0.4.11
 * Last Updated: 2009-07-02
 * Author : Chin Mi Ae, Lee Dae Beom
 * Copyright 2009 NHN Corp. All rights Reserved.
 * NHN PROPRIETARY/CONFIDENTIAL. Use is subject to license terms.
 *
 * This code includes some part of the
 * "Flash Player Detection Kit Revision 1.5" by Michael Williams
 * & Copyright 2005-2007 Adobe Macromedia Softward.LLC. All rights reserved.
 */


var lcs_isie = (navigator.appName == "Microsoft Internet Explorer"); 
var lcs_isns = (navigator.appName == "Netscape" );
var lcs_isopera = (navigator.appVersion.indexOf("Opera") >=  0 );
var lcs_ismac = (navigator.userAgent.indexOf("MAC")>=0); 

var lcs_add = {};
var lcs_bc = {};

var lcs_ver = "v0.4.11";
var lcs_count = 0;
lcs_obj = [];

function lcs_do( etc ) {
	// TODO : check lcs server name!! 
	if (!lcs_SerName) { var lcs_SerName = "lcs.naver.com"; }

	var rs = "";
	var index;

	try {
		// https 처리 되어 있음
		var lcs_Addr = (window.location.protocol ? window.location.protocol : "http:")+"//" + lcs_SerName + "/m?";
	} catch(e){ return; }

	try {
		rs = lcs_Addr + "u=" + encodeURIComponent(document.URL) 
			+ "&e=" + (document.referrer ? encodeURIComponent(document.referrer) : "");
	} catch(e) {
	}

	try {

	if (typeof lcs_add.i == 'undefined' )
		lcs_add.i = "";

	for( var index in lcs_add)
	{
		if( typeof lcs_add[index] != 'function' ) 
			rs += "&" + index + "=" + encodeURIComponent(lcs_add[index]);
	}

	for( var index in etc )
	{
		if ( (index.length >= 3 && (typeof etc[index] != 'function')) || index == 'qy')
		{
			rs += "&" + index + "=" + encodeURIComponent(etc[index]);
		}
	}

	
	lcs_getBrowserCapa();

	for( var index in lcs_bc )
	{
		if( typeof lcs_bc[index] != 'function' ) 
			rs += "&" + index + "=" + encodeURIComponent(lcs_bc[index]);
	}

	if(lcs_count > 0 )
	{
		var timeStr = (new Date).getTime();
		rs += "&ts=" + timeStr;
	}
	rs += "&EOU";

	if (document.images) {
		var obj = (new Image());
		lcs_obj.push(obj);
		obj.src = rs;
	} else {
		document.write( '<im' + 'g sr' + 'c="' + rs + '" width="1" height="1" border="0" />');
	}
	lcs_count++;

	} catch(e) {
		return;
	}
}

function lcs_do_gdid( gdid , etc) {

	try {
		if (gdid) {
			lcs_add["i"] = gdid;

			if (etc){
				lcs_do(etc);
			} else {
				lcs_do();
			}
			
		}
	} catch (e) {
	}
}

function lcs_getBrowserCapa() {
	lcs_getOS();
	
	lcs_getlanguage();
	
	lcs_getScreen();

	lcs_getWindowSize();

	lcs_getColorDepth();

	lcs_getJavaEnabled();  

	lcs_getJavascriptVer();

	lcs_getCookieEnabled(); 

	lcs_getSwfVer();

	lcs_getSLVersion();

	lcs_getConnectType();

	lcs_getPlugIn();

}

function lcs_getOS() {
	var lcs_os = "";
	try {
		(navigator.platform ? lcs_os = navigator.platform : "");
	} catch (e) {
	}
	lcs_bc["os"] = lcs_os;
}

function lcs_getlanguage() {
	var lcs_ln = "";
	try {
		(navigator.userLanguage? lcs_ln = navigator.userLanguage : (navigator.language)? lcs_ln = navigator.language : "");
	} catch (e) {
	}

	lcs_bc["ln"] = lcs_ln;
}

function lcs_getScreen() {
	var lcs_sr = "";
	try {
		if ( window.screen && screen.width && screen.height)
		{
			lcs_sr = screen.width + 'x'+ screen.height;
		}
		else if ( window.java || self.java ) 
		{
			var sr = java.awt.Toolkit.getDefaultToolkit().getScreenSize();
			lcs_sr = sr.width + 'x' + sr.height;

		}
	} catch(e) {
		lcs_sr = "";
	}

	lcs_bc["sr"] = lcs_sr;
}


function lcs_getWindowSize() {
	lcs_bc["bw"] = '';
	lcs_bc["bh"] = '';
	try {
		lcs_bc["bw"] = document.documentElement.clientWidth ? document.documentElement.clientWidth : document.body.clientWidth;
		lcs_bc["bh"] = document.documentElement.clientHeight ? document.documentElement.clientHeight : document.body.clientHeight;
	}
	catch(e) {
	}
}

function lcs_getColorDepth(){
	lcs_bc["c"] = "";
	try {
		if (window.screen) {
			lcs_bc["c"] = screen.colorDepth ? screen.colorDepth : screen.pixelDepth;
		}
		else if (window.java || self.java ) {
			var c = java.awt.Toolkit.getDefaultToolkit().getColorModel().getPixelSize();
			lcs_bc["c"] = c;
		}
	} catch (e) {
		lcs_bc["c"] = "";
	}
}

function lcs_getJavaEnabled() { 
	lcs_bc["j"] = "";
	try {
		lcs_bc["j"]= navigator.javaEnabled() ? "Y":"N";
	} catch (e) {
	}

}

function lcs_getCookieEnabled() {
	lcs_bc["k"] = "";
	try {
		lcs_bc["k"]= navigator.cookieEnabled ? "Y":"N";
	} catch (e) {
	}

}

function lcs_getConnectType() {
	var lcs_ct = "";
	try {
		if ( lcs_isie && !lcs_ismac && document.body ) {
			var obj = document.body.addBehavior("#default#clientCaps");
			lcs_ct = document.body.connectionType;
			document.body.removeBehavior(obj);
		}
	} catch(e) {
	}

	lcs_bc["ct"] = lcs_ct;
}

function lcs_getJavascriptVer() {
	var j = "1.0";
	try {
		if(String && String.prototype) {
			j = "1.1";
			if(j.search)
			{
				j = "1.2";
				var dt = new Date, no = 0;
				if ( dt.getUTCDate)
				{
					j = "1.3";
					var i, ie = navigator.appVersion.indexOf('MSIE');
					if (ie > 0 ) 
					{
						var apv =  parseInt(i = navigator.appVersion.substring(ie+5));
						if (apv > 3) { n_apv = parseFloat(i); }
					}
					if(lcs_isie && lcs_ismac && apv >= 5) 
					{
						j = "1.4"; 
					}
					if(no.toFixed)
					{
						j = "1.5";
						var a = new Array;
						if (a.every)
						{
							j = "1.6";
							i = 0;
							var obj = new Object;
							var test = function(obj) { var i = 0; try {	i = new Iterator(obj)} catch(e) {} return i};
							i = test(obj);
							if(i && i.next) j = "1.7";
							
							if(a.reduce) j = "1.8";
							
						}
	
					}
				}
			}
		}
	} catch (e) {}
	lcs_bc["jv"] = j;
}

function lcs_getSwfVer(){
	var flashVer = ''; 
	var isWin = false;

	try {
		isWin = (navigator.appVersion.toLowerCase().indexOf("win") != -1) ? true : false;
   
		if (navigator.plugins != null && navigator.plugins.length > 0) {
			if (navigator.plugins["Shockwave Flash 2.0"] || navigator.plugins["Shockwave Flash"]) {
				var swVer2 = navigator.plugins["Shockwave Flash 2.0"] ? " 2.0" : "";
				var flashDescription = navigator.plugins["Shockwave Flash" + swVer2].description;
				var descArray = flashDescription.split(" ");
				var tempArrayMajor = descArray[2].split(".");           
				var versionMajor = tempArrayMajor[0];
				var versionMinor = tempArrayMajor[1];
				flashVer = parseInt(versionMajor,10) + "." + parseInt(versionMinor, 10);
			}
		}
		else if (navigator.userAgent.toLowerCase().indexOf("webtv/2.6") != -1) flashVer = "4.0";
		else if (navigator.userAgent.toLowerCase().indexOf("webtv/2.5") != -1) flashVer = "3.0";
		else if (navigator.userAgent.toLowerCase().indexOf("webtv") != -1) flashVer = "2.0";
		else if ( lcs_isie && isWin && !lcs_isopera ) {
		    var version = '';
		    var axo;

		    try {
       			axo = new ActiveXObject("ShockwaveFlash.ShockwaveFlash.7");
   		     	version = axo.GetVariable("$version");
		    } catch (e) {}

		    if (!version)
		    {
		        try {
       		    	axo = new ActiveXObject("ShockwaveFlash.ShockwaveFlash.6");
           		 	version = "WIN 6,0,21,0";
	            	axo.AllowScriptAccess = "always";
            		version = axo.GetVariable("$version");
        			} catch (e) {}
    		}

			if (!version)
			{
				try {
					axo = new ActiveXObject("ShockwaveFlash.ShockwaveFlash.3");
					version = "WIN 3,0,18,0";
					version = axo.GetVariable("$version");
				} catch (e) {}
			}
		  
			if (!version)
			{
				try {
					axo = new ActiveXObject("ShockwaveFlash.ShockwaveFlash");
					version = "WIN 2,0,0,11";
				} catch (e) {}
			}
		   
			if (version.indexOf(',') > 0 ) {
				version = version.replace( /%20/,'');
				version = version.replace( /[a-zA-Z]*[^0-9]/,'');
				var verArray = version.split(",");
				version = parseInt(verArray[0],10) + "." + parseInt(verArray[1],10);
			}
			flashVer = version;
		}
	} catch (e) {}
	
	lcs_bc["fv"] = flashVer;
}

function lcs_getSLVersion() {
	var lcs_sl = "";

	try {
		if (navigator.plugins && navigator.plugins.length > 0 )
		{
			lcs_sl = navigator.plugins["Silverlight Plug-In"].description || navigator.plugins["WPFe Plug-In"].description;
			if (lcs_sl == '1.0.30226.2') lcs_sl = '2.0.30226.2';
		}
		else 
		{
			var wrap, obj;
			if (typeof ActiveXObject != 'undefined') {
				try { obj = new ActiveXObject('AgControl.AgControl'); 
				} catch(e) {}
			} else {
				wrap = document.createElement('div');
				wrap.innerHTML = '<object type="application/x-silverlight" style="position:absolute;" />';
				document.body.insertBefore(wrap, document.body.firstChild);
				obj = wrap.firstChild;
			}

			if (/\bopera\b/i.test(navigator.userAgent)) 
				for (var start = new Date().getTime(); typeof obj.isVersionSupported == 'undefined' && (new Date().getTime() - start < 1000); );
	
			if (typeof obj.isVersionSupported != 'undefined') {
	
				for (var major = 0; major < 9; major++) {
		
					for (var minor = 0; minor <= 9; minor++) {
						var v = major + '.' + minor;
						if (obj.isVersionSupported(v)) {
							lcs_sl = v;
						}
						else break;
		
					}
		
				}
				
			}
			
			if (obj) obj = null;
			if (wrap) document.body.removeChild(wrap);
		}

		if ( lcs_sl.indexOf('.') > 0 ) {
			var verArray = lcs_sl.split('.');
			lcs_sl = verArray[0] + '.' + verArray[1];
		}
	} catch(e) { }

	lcs_bc["sl"] =  lcs_sl;
}


function lcs_getPlugIn() {
	var plArr = {};
	var lcs_p = "";

   	if (navigator.plugins && navigator.plugins.length > 0)
	{
		try {
			var piArr = navigator.plugins;
			for (var i = 0; i < piArr.length; i++)
			{
				plArr[piArr[i].name] = '';		
			}
		} catch (e) {
		}
	} else {
		try {
			if (lcs_bc['fv'] != '' )
				plArr["Shockwave Flash"] = '';

			if (lcs_bc['sl'] != '' )
				plArr["Silverlight"] = '';
		} catch (e) {
		}

	    try {
			if (new ActiveXObject("SWCtl.SWCtl")) { plArr["Shockwave Director"] = '';}
        } catch (e) {
        }

	    try {
			if (new ActiveXObject("rmocx.RealPlayer G2 Control")
				|| new ActiveXObject("RealPlayer.RealPlayer(tm) ActiveX Control (32-bit)") 
				|| new ActiveXObject("RealVideo.RealVideo(tm) ActiveX Control (32-bit)")) {
				plArr["RealPlayer"] = '';
			}
        } catch (e) {
        }

		try {
			var index = navigator.userAgent.indexOf('MSIE');
			if (index != -1)
			{ 
				var ie_ver = parseFloat(navigator.userAgent.substring(index + 4 + 1));
				if (ie_ver < 7 ){
					if (new ActiveXObject("QuickTime.QuickTime")) {
						plArr["QuickTime"] = '';
					}

					if (new ActiveXObject("MediaPlayer.MediaPlayer.1")) { 
						plArr["Windows Media"] = '';
					} else {
						var obj_item = document.getElementsByTagName("object");	
						for(var i=0; i <  obj_item.length ; i++ ) {
							if(obj_item[i].classid) {
								var clsid = obj_item[i].classid.toUpperCase();
 								if ( clsid == "CLSID:6BF52A52-394A-11D3-B153-00C04F79FAA6" 
											|| clsid == "CLSID:22D6F312-B0F6-11D0-94AB-0080C74C7E95" ) {
									if (new ActiveXObject("MediaPlayer.MediaPlayer.1")) {
										plArr["Windows Media"] = '';
									}
								}
		
							}
								
						}
					}
				}
			}
		} catch (e) {
		}
	}

	for( var index in plArr ) {
		if( typeof plArr[index] != 'function' ) 
		lcs_p += index + ';';
	}

	lcs_bc["p"] = lcs_p.length ? lcs_p.substr(0, lcs_p.length-1) : lcs_p;
}


if(typeof nclk=="undefined"){nclk={}}if(typeof nclkMaxDepth=="undefined"){var nclkMaxDepth=8}if(typeof ccsrv=="undefined"){var ccsrv="cc.naver.com"}if(typeof nclkModule=="undefined"){var nclkModule="cc"}if(typeof nsc=="undefined"){var nsc="decide.me"}if(typeof g_pid=="undefined"){var g_pid=""}if(typeof g_sid=="undefined"){var g_sid=""}var nclkImg=[];if(typeof nclkMaxEvtTarget=="undefined"){var nclkMaxEvtTarget=4}if(typeof nclk_evt=="undefined"){var nclk_evt=0}nclk.nclktagVersion="1.0.10";nclk.addEvent=function(e,b,a){if(e.addEventListener){e.addEventListener(b,a,false)}else{if(e.attachEvent){e["e"+b+a]=a;e[b+a]=function(){e["e"+b+a](window.event)};e.attachEvent("on"+b,e[b+a])}}};nclk.generateCC=function(l){var r=l||window.event;if(!r){return false}var f=r.target||r.srcElement;var o=f.nodeName;var m,p;var q;var b="",t="",k="",g="";var a=0,n=0;var h,s;var i;var j=-1;if(r.button==2){return}if(f.nodeType==3){f=f.parentNode}if(f.parentNode&&f.parentNode.nodeName=="A"){f=f.parentNode}p=f;while(j<=nclkMaxEvtTarget){if(j>=nclkMaxEvtTarget){if(nclk_evt==2||nclk_evt==4){h=0;m=p;break}else{return}}else{i=nclk.getTag(f);h=i[0];s=i[1];if(h==0){if(f.parentNode){f=f.parentNode;j++}else{h=0;m=p;break}}else{m=f;break}}}switch(h){case 0:case 1:case 2:case 3:if(nclk_evt==2||nclk_evt==4){b="ncs.blank"}else{return}break;case 4:b=nclk.findArea(m,1);if(b==undefined){b=""}q=nclk.parseNCStr(h,s);b=b+"."+q[0];break;case 5:b=nclk.findArea(m,2);if(b==undefined){b=""}q=nclk.parseNCStr(h,s);break;case 6:q=nclk.parseNCStr(h,s);b=q[0];break;default:return}if(h==4||h==5||h==6){k=q[1];t=q[2];g=q[3];n=q[4]}if(n=="2"){return}else{a=n}if(g){clickcr(m,b,t,k,r,a,g)}else{clickcr(m,b,t,k,r,a)}};nclk.searchNextObj=function(a){var b=a.nextSibling;if(b&&b.nodeType==3){b=b.nextSibling}return b};nclk.getTag=function(g){var b=0;if(!g){return 0}var i;var f;var h;var a="";if(nclk_evt==1||nclk_evt==2){var e=nclk.searchNextObj(g);if(e){if(e!=null&&e.nodeType==8&&e.data.indexOf("=")>0){a=nclk.trim(e.data)}else{return[0,""]}}else{return[0,""]}}else{if(nclk_evt==3||nclk_evt==4){if(g.className){a=nclk.getClassTag(g.className);if(!a){return[0,""]}}else{return[0,""]}}}a=nclk.trim(a);i=a.split("=");f=i[0].charAt(0);h=i[0].substring(1);if(f!="N"){return[0,""]}if(h=="E"){b=1}else{if(h=="I"){b=2}else{if(h=="EI"||h=="IE"){b=3}else{if(h=="IP"||h=="PI"){b=4}else{if(h=="P"){b=5}else{if(i[0].length==1){b=6}else{b=0}}}}}}return[b,a]};nclk.findArea=function(b,h){var j=0;var g;var k;var m,f;var e="";var a=0;var l;var i;if(!b){return}if(h==1){a=1}else{if(h==2){a=0}}while(b=b.parentNode){g=b;while(1){if(nclk_evt==1||nclk_evt==2){g=g.previousSibling;if(g){if(g.nodeType==8){k=nclk.trim(g.data)}else{continue}}else{break}}else{if(nclk_evt==3||nclk_evt==4){k=b.className;if(k){k=nclk.getClassTag(k)}else{break}}}if(k.indexOf("=")>0){m=k.split("=");if(m[0].charAt(0)!="N"){continue}i=m[0].substring(1);if(i=="I"&&a==0){f=m[1].split(":");if(f[0]=="a"){if(f[1]!=""&&f[1]!=undefined){e=f[1]}}a++;break}else{if(i=="E"&&a==1){if(a==1){f=m[1].split(":");if(f[0]=="a"){if(e==""){e=f[1]}else{e=((f[1]==undefined)?"":f[1])+"."+e}}}a++;break}else{if((i=="EI"||i=="IE")&&a==0){f=m[1].split(":");if(f[0]=="a"){e=f[1]}a+=2;break}}}}if(nclk_evt==3||nclk_evt==4){break}}j++;if(a>=2){l=e;break}if(j>=nclkMaxDepth){l="";break}}return l};nclk.getServiceType=function(){var a;if(typeof g_ssc!="undefined"&&typeof g_query!="undefined"){a=1}else{a=0}return a};nclk.parseNCStr=function(h,o){var a;var m;var l;var e;var b="",k="",p="",f="",n=0;var g=2;o=nclk.trim(o);switch(h){case 4:g=4;break;case 5:g=3;break;case 6:g=2;break;case 1:case 2:case 3:default:return}m=o.substring(g);l=m.split(",");for(var j=0;j<l.length;j++){e=l[j].split(":");if(e[0]=="a"){b=e[1]}else{if(e[0]=="r"){k=e[1]}else{if(e[0]=="i"){p=e[1]}else{if(e[0]=="g"){f=e[1]}else{if(e[0]=="t"){n=e[1]}}}}}}return[b,k,p,f,n]};nclk.trim=function(a){return a.replace(/^\s\s*/,"").replace(/\s\s*$/,"")};nclk.getClassTag=function(g){var f="";if(typeof(g)=="string"){f=g}else{if(g.baseVal){f=g.baseVal}else{f=""+g}}var b=new RegExp("N[^=]*=([^ $]*)");var e=f.match(b);var a="";if(e){a=e[0]}return a};function nclk_do(){if(nclk_evt==1||nclk_evt==2||nclk_evt==3||nclk_evt==4){nclk.addEvent(document,"click",nclk.generateCC)}}nclk.getScrollBarWidth=function(){var e=document.createElement("p");e.style.width="200px";e.style.height="200px";var f=document.createElement("div");f.style.position="absolute";f.style.top="0px";f.style.left="0px";f.style.visibility="hidden";f.style.width="200px";f.style.height="150px";f.style.overflow="hidden";f.appendChild(e);document.body.appendChild(f);var b=e.offsetWidth;f.style.overflow="scroll";var a=e.offsetWidth;if(b==a){a=f.clientWidth}document.body.removeChild(f);return(b-a)};nclk.findPos=function(b){var f=curtop=0;try{if(b.offsetParent){do{f+=b.offsetLeft;curtop+=b.offsetTop}while(b=b.offsetParent)}else{if(b.x||b.y){if(b.x){f+=b.x}if(b.y){curtop+=b.y}}}}catch(a){}return[f,curtop]};nclk.windowSize=function(e){if(!e){e=window}var a=0;if(e.innerWidth){a=e.innerWidth;if(typeof(e.innerWidth)=="number"){var b=nclk.getScrollBarWidth();a=e.innerWidth-b}}else{if(document.documentElement&&document.documentElement.clientWidth){a=document.documentElement.clientWidth}else{if(document.body&&(document.body.clientWidth||document.body.clientHeight)){a=document.body.clientWidth}}}return a};nclk.checkIframe=function(i){var f=document.URL;var h=i.parentNode;var a;var b;if(h==null||h==undefined){return false}while(1){if(h.nodeName.toLowerCase()=="#document"){if(h.parentWindow){a=h.parentWindow}else{a=h.defaultView}try{if(a.frameElement!=null&&a.frameElement!=undefined){if(a.frameElement.nodeName.toLowerCase()=="iframe"){b=a.frameElement.id;if(!b){return false}return b}else{return false}}else{return false}}catch(g){return false}}else{h=h.parentNode;if(h==null||h==undefined){return false}}}};nclk.absPath=function(a){var e=window.location;var f=e.href;var b=e.protocol+"//"+e.host;if(a.charAt(0)=="/"){if(a.charAt(1)=="/"){return e.protocol+a}else{return b+a}}if(/^\.\//.test(a)){a=a.substring(2)}while(/^\.\./.test(a)){if(b!=f){f=f.substring(0,f.lastIndexOf("/"))}a=a.substring(3)}if(b!=f){if(a.charAt(0)!="?"&&a.charAt(0)!="#"){f=f.substring(0,f.lastIndexOf("/"))}}if(a.charAt(0)=="/"){return f+a}else{if(a.charAt(0)=="?"){f=f.split("?")[0];return f+a}else{if(a.charAt(0)=="#"){f=f.split("#")[0];return f+a}else{return f+"/"+a}}}};function clickcr(g,H,u,D,E,A,P){if(arguments.length==1){if(typeof nclk.generateCC!="undefined"){nclk.generateCC(arguments[0])}return}var F=navigator.userAgent.toLowerCase();var k=(F.indexOf("safari")!=-1?true:false);var C=/msie/.test(F)&&!/opera/.test(F);var l=(window.location.protocol=="https:")?"https:":"http:";var a=ccsrv.substring(ccsrv.indexOf(".")+1);var t=window.event?window.event:E;var s=-1;var q=-1;var p=-1;var n=-1;var S,f,i;var r,j,m;var M,J,I,L,z,B,w;var O;var G=0;if(!A){A="0"}else{A=String(A)}if(!P){P=""}if(A.indexOf("n")==0){G=0}else{if(window.g_ssc!=undefined&&window.g_query!=undefined){G=1}else{G=0}}try{L=nclk.windowSize(window);i=nclk.checkIframe(g);if(i){var v=nclk.findPos(document.getElementById(i));if(t.clientX&&t.clientX!=undefined){S=document.body;if(S.clientLeft&&S.clientTop){ifrSx=t.clientX-S.clientLeft;ifrSy=t.clientY-S.clientTop}else{ifrSx=t.clientX;ifrSy=t.clientY}}p=v[0]+ifrSx;n=v[1]+ifrSy;if(document.body&&(document.body.scrollTop||document.body.scrollLeft)){S=document.body;s=p-S.scrollLeft;q=n-S.scrollTop}else{if(document.documentElement&&(document.documentElement.scrollTop||document.documentElement.scrollLeft)){f=document.documentElement;s=p-f.scrollLeft;q=n-f.scrollTop}else{s=p;q=n}}}else{if(t.clientX&&t.clientX!=undefined){S=document.body;if(S.clientLeft&&S.clientTop){s=t.clientX-S.clientLeft;q=t.clientY-S.clientTop}else{s=t.clientX;q=t.clientY}}if(document.body&&(document.body.scrollTop||document.body.scrollLeft)){p=document.body.scrollLeft+(s<0?0:s);n=document.body.scrollTop+(q<0?0:q)}else{if(document.documentElement&&(document.documentElement.scrollTop||document.documentElement.scrollLeft)){f=document.documentElement;if(f.scrollLeft!=undefined){p=f.scrollLeft+(s<0?0:s)}if(f.scrollTop!=undefined){n=f.scrollTop+(q<0?0:q)}}else{p=(s<0?0:s);n=(q<0?0:q)}}if(t.pageX){p=t.pageX}if(t.pageY){n=t.pageY}}}catch(Q){}if(H==""||typeof H=="undefined"){return}if(A.indexOf("1")!=-1){r=0}else{if(g.href){z=g.nodeName.toLowerCase();B=g.href.toLowerCase();if((g.target&&g.target!="_self"&&g.target!="_top"&&g.target!="_parent")||(B.indexOf("javascript:")!=-1)||(g.getAttribute("href",2)&&g.getAttribute("href",2).charAt(0)=="#")||(B.indexOf("#")!=-1&&(B.substr(0,B.indexOf("#"))==document.URL))||z.toLowerCase()=="img"||C&&window.location.host.indexOf(a)==-1){r=0}else{r=1}}else{r=0}}if(g.href&&g.href.indexOf(l+"//"+ccsrv)==0){j=g.href}else{j=l+"//"+ccsrv+"/"+nclkModule+"?a="+H+"&r="+D+"&i="+u;j+="&bw="+L+"&px="+p+"&py="+n+"&sx="+s+"&sy="+q+"&m="+r;if(G==0){j+="&nsc="+nsc}else{if(G==1){j+="&ssc="+g_ssc+"&q="+encodeURIComponent(g_query)+"&s="+g_sid+"&p="+g_pid}}if(P){j+="&g="+encodeURIComponent(P)}if(B&&B.indexOf(l+"//"+ccsrv)!=0&&z.toLowerCase()!="img"){var N=g.href;if(g.outerHTML&&!window.XMLHttpRequest){N=(/\shref=\"([^\"]+)\"/i.test(g.outerHTML)&&RegExp.$1).replace(/\\/g,"\\\\").replace(/%([A-Z0-9]{2})/ig,"\\$1");(d=document.createElement("div")).innerHTML=N;N=d.innerText.replace(/\\([A-Z0-9]{2})/gi,"%$1").replace(/\\\\/g,"\\")}B=N.toLowerCase();if(B.indexOf("http:")==0||B.indexOf("https:")==0||B.indexOf("javascript:")==0){j+="&u="+encodeURIComponent(N)}else{w=nclk.absPath(N);j+="&u="+encodeURIComponent(w)}}else{if(g.href){if(g.href.length>0){j+="&u="+encodeURIComponent(g.href)}else{j+="&u=about%3Ablank"}}else{j+="&u=about%3Ablank"}}}if(r==1){O=g.innerHTML;g.href=j;if(g.innerHTML!=O){g.innerHTML=O}}else{if(document.images){var K=new Date().getTime();j+="&time="+K;if(k&&!g.href){var R=c=new Date();while((R.getTime()-c.getTime())<100){R=new Date()}var M=new Image();nclkImg.push(M);M.src=j}else{var M=new Image();nclkImg.push(M);M.src=j}}}return true};
//domain setting
document.domain = 'game.naver.com';

// -----------------------------------------------------------------------------
// envs
var gdp = {
	envPhase: '',
	prefix: '',
	gdpApiUrl: '',
	gameApiUrl: '',
	wwwUrl: '',
	idpLoginUrl: '',
	idpLogoutUrl: '',
	naverCashChargeUrl: '',
	
	gameId: '',
	
	isValidGame: false,
	isMirrorActivated: false,
	isLoggedIn: false,
	isNeedSyncCookie: false,
	
	isCorporateUser: false,
	isNeedRealNameCheck: false,
	isAgreeNaverGameService: false,
	
	gnbMarkupJson : null,
	gnbMarkupCssJson: null,
	
	errorMsg: '',
	destUrl: '',
	
	isQA: false
};

//base internal functions
gdp.getCookie = function(s) {
	var v, c = document.cookie.split("; ");
	for (var i=0; i<c.length; i++) {
		v = c[i].split("=");
		if (s.toLowerCase() == unescape(v[0].toLowerCase())) {
			if (v.length > 2) {
				var	s = v[1];			
				for (var i=2;i<v.length;i++) s = s + "=" + v[i];
				return s;
			}
			return v[1];
		}
	}
	return "";
};


gdp.setCookie = function (cName, cValue, cDay){
    var expire = new Date();
    expire.setDate(expire.getDate() + cDay);
    cookies = cName + '='+ escape(cValue) + 
    		'; path=/ ; expires=' + expire.toGMTString() + ';'
    		+ 'domain=' + '.game.naver.com;';
    document.cookie = cookies;
};

gdp.createIframeObject = function(id) {	
	var frame = document.getElementById(id);
	if (frame == null)	{
		var obj = document.createElement('iframe');
		obj.setAttribute('id', id);
		obj.style.border = '0px';
		obj.style.width = '0px';
		obj.style.height = '0px';
		obj.style.visibility = 'visible';		
		frame = document.body.appendChild(obj);		
	}
	return frame;
};



if (typeof gdp == 'undefined') gdp = {};
gdp.CircularRoller = function($j, el, type){
	var obj = {};
	
	var wrapper = el;
	var ul = wrapper.find('ul');
	
	wrapper.css('position', 'relative');
	ul.css('position', 'absolute');
	
	var rollerType = type;
	var viewDim = (rollerType == 'horizontal') ? wrapper.width() : wrapper.height();
	var itemCount = wrapper.find('li').length;
	var isInTransition = false;
	
	var getDimProperty = function() {
		return (rollerType == 'horizontal') ? 'left' : 'top';
	};
	
	//zero based index
	var getIndex = function() {
		var offset = (rollerType == 'horizontal') ? ul.position().left : ul.position().top;
		var index = Math.abs(Math.round(offset/viewDim));
		
		return index;
	};
	
	var doMoveBy = function(direction, callback, transOffset) {
		var moveOffset = ((transOffset) ? transOffset : viewDim);
		var option = {};
		var sign = '+=';
		if (direction == 1) {
			 sign = '-=';
		}
		
		option = (rollerType == 'horizontal') ? (option = {'left' : (sign + moveOffset)}) : (option = {'top' : (sign + moveOffset)});
		
		isInTransition = true;
		ul.animate(option, 200, function(){
			if(callback){
				callback();
			}
			isInTransition = false;
		});
	};
	
	obj.getIndex = function(){
		return getIndex();
	};
	
	obj.moveBy = function(direction, afterMoveCallback){
		if (isInTransition) {
			return;
		}
		
		var index = getIndex();
		
		//next
		if (direction == 1) {
			if (index == itemCount -1) {
				wrapper.find('li:first').clone().appendTo(ul);
				ul.css(getDimProperty(), '-' + (viewDim * (itemCount - 1)) + 'px');
				doMoveBy(direction, function(){
					ul.css(getDimProperty(), '0px');
					wrapper.find('li:last').remove();
					if(afterMoveCallback) afterMoveCallback();
				});
				return;
			}
			
			doMoveBy(direction, function(){
				if(afterMoveCallback) afterMoveCallback();
			});
			return;
		}
		
		//prev
		if (direction == -1) {
			if (index == 0) {
				wrapper.find('li:last').clone().prependTo(ul);
				ul.css(getDimProperty(), '-' + viewDim + 'px');
				doMoveBy(direction, function(){
					ul.css(getDimProperty(), '-' + (viewDim * (itemCount - 1)) + 'px');
					wrapper.find('li:first').remove();
					if(afterMoveCallback) afterMoveCallback();
				});
				return;
			}
			
			doMoveBy(direction, function(){
				if(afterMoveCallback) afterMoveCallback();
			});
			return;
		}
		
	};
	
	obj.moveTo = function(targetIndex, afterMoveCallback) {
		if (isInTransition) {
			return;
		}
		
		var index = getIndex();
		var direction = 1;
		
		if (targetIndex == index) {
			return;
		}
		
		if (targetIndex < index) {
			direction = -1;
		}
		
		var diff = Math.abs(targetIndex - index);
		doMoveBy(direction, function(){
			if(afterMoveCallback) afterMoveCallback();
		}, (diff * viewDim));
	};
	
	return obj;
	
	
};

gdp.Promo = (function($j){
	var obj = {};
	var noticeRoller;
	var promoRoller;
	var promoNavi;
	var foldTimer;
	
	//열기
	var unfoldPromo = function(){
		if ($j('.global_promo').is(':visible')){
			return;
		}
		
		$j('.promo_wrap').css('height', '0');
		$j('.global_promo').show();
		
		$j('.promo_wrap').animate({
			'height' : '230'
		}, 200, function(){
			
		});
	};
	
	//닫기
	var foldPromo = function(){
		if (!$j('.global_promo').is(':visible')){
			return;
		}
		
		$j('.promo_wrap').animate({
			'height' : '0'
		}, 200, function(){
			$j('.global_promo').hide();
		});
	};
	
	var setPromoNav = function(){
		var index = promoRoller.getIndex();
		var as = promoNavi.find('a');
		var targetA = as.eq(index);
		
		as.removeClass('on');
		targetA.addClass('on');
	};
	
	var initHandler = function(){
		notiSection.find('.noti_prev').click(function(){
			noticeRoller.moveBy(-1);
			promoRoller.moveBy(-1, function(){
				setPromoNav();
			});
		});
		notiSection.find('.noti_next').click(function(){
			noticeRoller.moveBy(1);
			promoRoller.moveBy(1, function(){
				setPromoNav();
			});
		});
		
		promoSection.find('.pro_prev').click(function(){
			noticeRoller.moveBy(-1);
			promoRoller.moveBy(-1, function(){
				setPromoNav();
			});
		});
		promoSection.find('.pro_next').click(function(){
			noticeRoller.moveBy(1);
			promoRoller.moveBy(1, function(){
				setPromoNav();
			});
		});
		
		promoNavi.find('a').click(function(){
			var as = promoNavi.find('a');
			var el = $j(this);
			var index = as.index(el);

			noticeRoller.moveTo(index);
			promoRoller.moveTo(index, function(){
				setPromoNav();
			});
		});
		
		promoSection.mouseover(function(){
			promoSection.find('.pro_prev').show();
			promoSection.find('.pro_next').show();
		}).mouseout(function(){
			promoSection.find('.pro_prev').hide();
			promoSection.find('.pro_next').hide();
		});
		
		notiSection.mouseenter(function(){
			unfoldPromo();
		});
		
		promoSection.hover(function(){
			if (foldTimer) {
				clearTimeout(foldTimer);
				foldTimer = null;
			}
		}, function(){
			if (foldTimer) {
				clearTimeout(foldTimer);
			}
			
			foldTimer = setTimeout(foldPromo, 100);
		});
		
		$j('.global_wrap .global_inr').hover(function(){
			if (foldTimer) {
				clearTimeout(foldTimer);
				foldTimer = null;
			}
		}, function(){
			if (foldTimer) {
				clearTimeout(foldTimer);
			}
			
			foldTimer = setTimeout(foldPromo, 100);
		});
		
		promoSection.find('.pro_prev').hide();
		promoSection.find('.pro_next').hide();
		
	};
	
	obj.init = function(){
		notiSection = $j('.global_wrap .noti_section');
		promoSection = $j('.global_wrap .global_promo');
		
		
		noticeRoller = new gdp.CircularRoller($j, notiSection.find('.noti_wrap'), 'vertical');
		promoRoller = new gdp.CircularRoller($j, promoSection.find('.promo_wrap'), 'horizontal');
		
		
		promoNavi = promoSection.find('.pagination_bnr');
		
		initHandler();
	};
	
	return obj;
	
}(jQuery));

gdp.jslib = (function($j){
	var obj = {};
	
	var IMAGES_TYPES = ['42x42', '60x60', '100x78', '122x96', '130x102', '186x130','226x86','258x180', '928x270'];
	var IMAGES_CATEGORIES = ['cafe', '42x42_1'];
	var IMAGES_CATEGORY_IMAGES;
	if( window.location.protocol == "https:"){
		IMAGES_CATEGORY_IMAGES = ['https://imgsec.playnetwork.co.kr/nvgame/img/img_cafe.jpg', 'https://imgsec.playnetwork.co.kr/nvgame/upload/thumb/default_42x42_1.jpg'];
	}
	else{
		IMAGES_CATEGORY_IMAGES = ['https://images.playnetwork.co.kr/nvgame/img/img_cafe.jpg', 'https://images.playnetwork.co.kr/nvgame/upload/thumb/default_42x42_1.jpg'];
	}
	var openWin = function(sURL, sWindowName, w, h)
	{
		var sOption = "";
		sOption = sOption + "toolbar=no, channelmode=no, location=no, directories=no, menubar=no";
		sOption = sOption + ", scrollbars=no, width=" + w + ", height=" + h;

		var win = window.open(sURL, sWindowName, sOption);
		return win;
	};
	
	function getNotiCookie( name ){
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
	function OpenPopup() {  
	      window.open("https://game.naver.com/common/noticeGameCash.jsp","noticeGameCash","top=0,left=0,width=536,height=350,resizable=1,scrollbars=no");
	}
	
	var gameCashPopUp = function() {
		if( getNotiCookie( "noticeGameCashClose" ) == '' ){
			OpenPopup();
		}
	};
	
	
	var initMyGameSection = function() {
		var wrapper = $j('.global_wrap .myinfo_section');
		
		//toggle layer
		wrapper.find('a.more').click({wrapper: wrapper}, function(event){
			event.preventDefault();
			
			event.data.wrapper.toggleClass('on');
		});
		
		//layer prev/next button
		if (wrapper.find('.rcnt_wrap ul.rcnt_lst').length > 1) {
			wrapper.find('.rcnt_prev').click(function(event){
				event.preventDefault();
				
				var uls = wrapper.find('.rcnt_wrap ul.rcnt_lst');
				var visibleUl = wrapper.find('.rcnt_wrap ul.rcnt_lst:visible');
				var ulIndex = uls.index(visibleUl);
				
				if (ulIndex > 0) {
					uls.hide();
					uls.eq(ulIndex - 1).show();
				}
			});
			
			wrapper.find('.rcnt_next').click(function(event){
				event.preventDefault();
				
				var uls = wrapper.find('.rcnt_wrap ul.rcnt_lst');
				var visibleUl = wrapper.find('.rcnt_wrap ul.rcnt_lst:visible');
				var ulIndex = uls.index(visibleUl);
				
				if (ulIndex < uls.length -1) {
					uls.hide();
					uls.eq(ulIndex + 1).show();
				}
			});
		}
	};
	
	var initPromotion = function() {
		gdp.Promo.init();
	};
	
	var loadScripts = function() {
		var scriptSrc = gdp.gameApiUrl + '/share/js/thickbox.js';
		var scriptTag = '<script type="text/javascript" charset="utf-8" src="'+scriptSrc+'" ></script>';
		
		var cssSrc = gdp.gameApiUrl + '/share/js/thickbox.css';
		var cssTag = '<link type="text/css" rel="stylesheet" href="'+cssSrc+'" />';
		
		document.write(cssTag);
		document.write(scriptTag);
	};
	
	var syncCookie = function() {
		var nxtUrl = location.href;
		top.location.href = gdp.wwwUrl + '/login.nhn?nxtUrl=' + encodeURIComponent(nxtUrl);
	};
	
	var reloadPage = function() {
		$j.ajax({
	        url : gdp.gameApiUrl + '/js/jslib.nhn?gameId=RELOAD',
	        type : 'get',
	        cache : false,
	        headers : {"cache-control":"no-cache","pragma":"no-cache"},
	        dataType : "jsonp",
	        jsonp : "callback",
	        complete : function(result) {
	    		top.location.reload(true);
	        }
		});
	};
	
	/**
	 * 이미지를 가져오지 못하는 경우 기본 이미지를 출력하도록 처리하는 이벤트 핸들러
	 * category 및 image size 를 지원 
	 * @param el		함수를 호출한 img element
	 * @param category 	이미지 카테고리. 지정되지 않은 경우 게임의 썸네일인 것으로 간주함
	 */
	obj.imageLoadErrorHandler = function (el, category) {
		el.onerror = null;
		
		if (category) {
			var categoryIndex = $j.inArray(category || '', IMAGES_CATEGORIES);
			if (categoryIndex !== -1) {
				el.src = IMAGES_CATEGORY_IMAGES[categoryIndex];
			}
			return;
		} else {
			var type = el.getAttribute('width') + 'x'+ el.getAttribute('height');
			var index = $j.inArray(type, IMAGES_TYPES);
			
			var baseUrl;
			if (gdp.getEnvPhase === 'release') {
				baseUrl = 'https://images.playnetwork.co.kr/nvgame/upload/thumb/';
			} else {
				baseUrl = 'https://alpha-images.playnetwork.co.kr/nvgame/upload/thumb/';
			}
			
			var path;
			if (index !== -1) {
				path = 'default_' + type + '.jpg';
			} else {
				path = 'default_258x180.jpg';
			}
			
			el.src = baseUrl + path;
		}
	};
	
	//API functions
	obj.showGNB = function() {
		
		//0. set document Title
		document.title = "네이버 PC게임";
		
		//1. load markup
		
		document.write(gdp.gnbMarkupJson.html);
		
		//2. set values
		
		//3. lazy load scripts
		
		//4. set up handlers
		initMyGameSection();
		$j(function(){
			initPromotion();
		});
		lcs_do();
		nclk_do();
		
		// navigator.appVersion.indexOf("Mac") != -1
		
		if( (gdp.getCookie("userOSMAC") != "true") && (navigator.appVersion.indexOf("Mac") != -1) ){
			gdp.setCookie('userOSMAC','true',7);
			alert('윈도우 환경에서 원활한 이용이 가능합니다. 맥 환경에서는 게임의 실행 및 일부 기능 이용이 불가능할 수 있습니다.\n(이 메시지는 이후 7일간 표시되지 않습니다.)');
		}
	};
	
	obj.checkLogin = function() {
		
		return gdp.isLoggedIn;
	};
	
	obj.goHome = function() {
		location.href = gdp.wwwUrl + '/';
	};
	
	obj.goGame = function(gameNo, gameTypeCode) {
		if (gameTypeCode == 'PKG') {
			location.href = gdp.wwwUrl + '/game/pkg.nhn?pno=' + gameNo;
		} else if (gameTypeCode == 'APP') {
			location.href = gdp.wwwUrl + '/game/app.nhn?pno=' + gameNo;
		} else {
			location.href = gdp.wwwUrl + '/game/ong.nhn?pno=' + gameNo;
		}
	};
	
	obj.goMirror = function(gameNo, gameTypeCode) {
		if (gameTypeCode == 'ONG') {
			location.href = gdp.wwwUrl + '/game/mirror.nhn?pno=' + gameNo;
		} else if (gameTypeCode == 'APP') {
			location.href = gdp.wwwUrl + '/game/app.nhn?pno=' + gameNo;
		} 
		
		//TODO: (sjs)PKG, APP 추가 필요
	};
	
	obj.goLogin = function(targetUrl) {
		var nxtUrl = '';
		if(typeof(targetUrl) != 'undefined' && targetUrl != ''){
			nxtUrl = targetUrl;
		} else {
			nxtUrl = location.href;
		}
		
		var url = gdp.wwwUrl + '/login.nhn?nxtUrl=' + encodeURIComponent(nxtUrl);
		location.href = gdp.idpLoginUrl + '?url=' + encodeURIComponent(url);
	};
	
	obj.goLogout = function() {
		var nxtUrl = gdp.idpLogoutUrl + '?returl=' + encodeURIComponent(gdp.wwwUrl);
		
		top.location.href = gdp.wwwUrl + '/logout.nhn?nxtUrl=' + encodeURIComponent(nxtUrl);
	};
	
	obj.goLink = function(targetPageID) {
		if (gdp.isQA === "true") {
			alert("정상적으로 goLink 호출 되었습니다.");
		}
		
		if (targetPageID == 'Home') {
			location.href = gdp.wwwUrl + '/index.nhn';
		} else if(targetPageID == 'MyInfo') {
			location.href = 'https://nid.naver.com/user/help.nhn?todo=changeMain'; 
		} else if(targetPageID == 'FindID') {
			location.href = 'https://nid.naver.com/user/help.nhn?todo=idinquiry';
		} else if(targetPageID == 'FindPW') {
			location.href = 'https://nid.naver.com/user/help.nhn?todo=pwinquiry';
		} else if(targetPageID == 'JoinIDP') {
			location.href = 'https://nid.naver.com/user/join.html?url=https://www.naver.com';
		} else if(targetPageID == 'CoinHelp') {
			window.open('https://help.naver.com/support/contents/contents.nhn?serviceNo=801&categoryNo=3132', "coinHelp", "width=1100, height=850, scrollbars=yes, resizable=yes");
		} else if(targetPageID == 'CustomerCenter'){
			location.href = 'https://help.naver.com/support/service/main.nhn?serviceNo=801';
		} else if(targetPageID == 'NaverMileage'){
			window.open('https://mileage.naver.com','_blank');
		}
	};
	
	obj.ownershipCheck = function(type, redirectUrl, state) {
		
		var url = gdp.gameApiUrl + '/dialog/checkIdentificationPop.nhn?gameId='+gdp.gameId + '&type=' + type;
		if(redirectUrl != null){
			url += '&redirectUrl=' + encodeURIComponent(redirectUrl);
		}
		if(state != null) {
		    url += '&state=' + state;	
		}
		 
		if (type=='ipin') {
			window.open(url, 'ownershipCheck', 'resizable=yes,status=no,toolbar=no,menubar=no,scrollbars=no,width=445,height=550');
		} else {
			window.open(url, 'ownershipCheck', 'resizable=yes,status=no,toolbar=no,menubar=no,scrollbars=no,width=800,height=750');
		}
	};
	
	obj.popupChargeWindow = function(rurl) {
		var strRurl = '';
		
		if(rurl != null ){
			strRurl = '&RURL=' + encodeURIComponent(rurl);
		}
		
		if (gdp.isQA === "true") {
			var message = "정상적으로 popupChargeWindow 호출 되었습니다.";
			// "미러링웹url"일 경우, domain & gameId 유효성 확인
			$j.ajax({
		        url : gdp.gameApiUrl + '/js/ajax/checkDomainGameId.nhn?gameId=' + gdp.gameId,
		        type : 'get',
		        cache : false,
		        headers : {"cache-control":"no-cache","pragma":"no-cache"},
		        dataType : "jsonp",
		        jsonp : "callback",
		        success : function(result) {
		        	if( result.code == "SUCCESS" ){
		        		message += "\n\rDomain 과 gameId 정보가 유효합니다.";
		        		message += "\n\r( gameId : " + gdp.gameId + ")";
		        	}
		        	else if( result.code == "FAIL" ){
		        		message += "\n\r* 비정상적인 gameId 입니다.";
		        		message += "\n\r현재 Domain : " + document.location.host;
		        		message += "\n\r현재 gameId : " + gdp.gameId;
		        		message += "\n\r도메인에 해당하는 gameId : " + result.validGameId;
		        	}
		        	alert(message);
					openWin(gdp.naverCashChargeUrl+strRurl, 'charge', 350, 440);
		        }
		    });
		}
		else{
			openWin(gdp.naverCashChargeUrl+strRurl, 'charge', 350, 440);
		}
	};
	
	//TODO: remove this function!
	obj.showNaverServiceTerm = function() {
		$j('#agreeNaverGameLink').click();
	};
	
	obj.init = function(conf) {
		gdp.envPhase = conf.envPhase;
		gdp.prefix = conf.prefix;
		gdp.gameApiUrl = conf.gameApiUrl;
		gdp.wwwUrl = conf.wwwUrl;
		gdp.idpLoginUrl = conf.idpLoginUrl;
		gdp.idpLogoutUrl = conf.idpLogoutUrl;
		gdp.naverCashChargeUrl = conf.naverCashChargeUrl;
		
		gdp.gameId = conf.gameId;
		
		gdp.isValidGame = (conf.isValidGame == 'true') ? true : false;
		gdp.isMirrorActivated = (conf.isMirrorActivated == 'true') ? true : false;
		gdp.isLoggedIn = (conf.isLoggedIn == 'true') ? true : false;
		gdp.isNeedSyncCookie = (conf.isNeedSyncCookie == 'true') ? true : false;
		gdp.needReload = (conf.needReload == 'true') ? true : false;
		
		gdp.isCorporateUser = (conf.isCorporateUser == 'true') ? true : false;
		gdp.isNeedRealNameCheck = (conf.isNeedRealNameCheck == 'true') ? true : false;
		gdp.isAgreeNaverGameService = (conf.isAgreeNaverGameService == 'true') ? true : false;
		
		gdp.gnbMarkupJson = conf.gnbMarkupJson;
		
		gdp.errorMsg = conf.errorMsg || '';
		gdp.destUrl = conf.destUrl || '';
		
		gdp.isQA = conf.isQA || false;
		
		
		
		if (!gdp.isValidGame) {
			alert(gdp.errorMsg);
			top.location.href = gdp.destUrl;
		}
		
		if (!gdp.isMirrorActivated) {
			if(gdp.gameId == 'P_GDPMOCK')
				alert('샘플사이트 임시점점중입니다. 잠시후에 이용해 주시기 바랍니다');
			gdp.jslib.goHome();
		}
		
		if (gdp.isCorporateUser) {
			alert(gdp.errorMsg);
			top.location.href = gdp.destUrl;
		}
		
		if (gdp.isNeedRealNameCheck) {
			alert(gdp.errorMsg);
			top.location.href = gdp.destUrl;
		}
		
		if (gdp.isNeedSyncCookie) {
			syncCookie();
		}
		
		if (gdp.needReload) {
			reloadPage();
		}
		
		if(document.compatMode == 'BackCompat'){ //쿼크모드일경우
			document.write("<link rel='stylesheet' type='text/css' href='"+gdp.wwwUrl+"/share/css/global_gnb_quirk.css?r="+ new Date().getTime() + "'>");
		}else if(window.location.protocol == "https:"){
			document.write("<link rel='stylesheet' type='text/css' href='"+gdp.wwwUrl.replace("http:","https:")+"/share/css/global_gnb_ssl.css?r="+ new Date().getTime() + "'>");
		}else{
			document.write("<link rel='stylesheet' type='text/css' href='"+gdp.wwwUrl+"/share/css/global_gnb.css?r="+ new Date().getTime() + "'>");
		}
		
		/*if (gdp.isLoggedIn) {
			if (!gdp.isAgreeNaverGameService) {
				var agreeNaverGameLayerUrl = gdp.wwwUrl + '/game/gdpAgreeNaverGame.nhn?pno=' + gdp.gameId;
				var content = '<a id="agreeNaverGameLink" href="'+agreeNaverGameLayerUrl+'&keepThis=true&TB_iframe=true&height=500&width=500&modal=true" class="thickbox"></a>';
				
				document.write(content);
				
				loadScripts();
			}
		}*/
	};
	
	return obj;
	
}(jQuery));




var nsc = "game.mirror";
var ccsrv = "cc.naver.com";
var nclk_evt = 3;

gdp.jslib.init({
    envPhase: 'release',
    prefix: '',
    gameApiUrl: 'https://api.game.naver.com',
    wwwUrl: 'https://game.naver.com',
    idpLoginUrl: 'https://nid.naver.com/nidlogin.login',
    idpLogoutUrl: 'https://nid.naver.com/nidlogin.logout',
    naverCashChargeUrl: 'https://p.billg.naver.com/pay/charge.nhn?CHNL=NG',

    gameId: 'P_PN025542',
    
    isValidGame: 'true',
    isMirrorActivated: 'true',
    isLoggedIn: 'true',
    isNeedSyncCookie: 'false',
    needReload: '',
    
    isCorporateUser: 'false',
    isNeedRealNameCheck: 'false',
    isAgreeNaverGameService: 'false',
    
    gnbMarkupJson: {"html":"\r\n\r\n\r\n\r\n\r\n\r\n<div class=\"global_wrap\">\r\n\t<div class=\"global_inr\">\r\n\t<div class=\"global_gnb\">\r\n\t    \r\n\t    <div id=\"gnbCouponBannerContainer\" class=\"ly_coupon2\" style=\"display:none;\">\r\n            <a id=\"gnbCouponBannerLink\" href=\"javascript:;\" target=\"_blank\"></a>\r\n\t    </div>\r\n        <div class=\"sta_inr\">\r\n            <h1><a href=\"https://www.naver.com\" class=\"logo_naver N=a:STA.naver\">NAVER</a> <a href=\"javascript:;\" class=\"logo_game N=a:STA.game\" onclick=\"gdp.jslib.goHome();\">GAME</a></h1>\r\n            <div class=\"noti_section\">\r\n                <h2 class=\"blind\">공지사항</h2>\r\n                <div class=\"noti_wrap\">\r\n                \t<ul>\r\n                \t\r\n                    <li><p><a href=\"https://tr.playnetwork.co.kr/event/2017/05/01_devblockmap/event.asp\">개발자 블록맵 콘텐스트</a></p></li>\r\n                \t\r\n                    <li><p><a href=\"https://origin.darkeden.game.naver.com/?view=ok\">죽이는 MMORPG의 귀환</a></p></li>\r\n                \t\r\n                    <li><p><a href=\"https://tr.playnetwork.co.kr/event/2016/12/14_Seventeen/index.asp\">신개념 FPS 게임!</a></p></li>\r\n                \t\r\n                    <li><p><a href=\"https://df.game.naver.com/df/pg/wpriest2nd?intro=yes\">액션쾌감!!!</a></p></li>\r\n                \t\r\n                    <li><p><a href=\"https://closers.nexon.game.naver.com/events/2017/02/02/OpenEvent.aspx\">클로저스</a></p></li>\r\n                \t\r\n                    </ul>\r\n                </div>\r\n\t\t\t\t<a href=\"javascript:;\" class=\"noti_prev\">이전 공지사항 보기</a>\r\n                <a href=\"javascript:;\" class=\"noti_next\">다음 공지사항 보기</a>\r\n            </div>\r\n            \r\n            <!-- 미러 gnb 135x60 배너 영역 -->  \r\n            <div class=\"pro_bnr\"> \r\n                \r\n                 \r\n            </div> \r\n            <!-- //미러 gnb 135x60 배너 영역 -->\r\n        </div>\r\n\r\n\t\t<div class=\"fr\">\r\n\t\t\r\n        \r\n\r\n\r\n\r\n\r\n\r\n\r\n        \r\n        <input type=\"hidden\" id=\"wwwUrl\" value=\"https://game.naver.com\">\r\n        \r\n        \r\n        <!-- 내 결제내역 보기 -->\r\n        <!-- [D] 레이어 노출시 show 클래스 추가  -->\r\n        <div class=\"cash_info\">\r\n\t\t\t<a href=\"javascript:;\" class=\"cash\">내 결제내역 보기</a>\r\n           \t<div class=\"ly_cash\">            \r\n            </div>\r\n        </div>\r\n        <!-- //내 결제내역 보기 -->\r\n        \r\n        <!-- [D] 로그인 전에 bf 클래스 추가 / 로그인 후 bf 클래스 삭제 -->\r\n        <div class=\"prof_info\">\r\n        \t<a href=\"https://game.naver.com/cash/myGameCash.nhn\" class=\"nick N=a:GNB.profile\"><strong>SilverMirror님</strong></a>\r\n\t\t\t<a href=\"javascript:;\" class=\"login\" onclick=\"gdp.jslib.goLogout();\"><img src=\"https://images.playnetwork.co.kr/nvgame/img/btn_logout.gif\" width=\"53\" height=\"19\" alt=\"로그아웃\"></a>\r\n\t\t\t<ul class=\"cash_lst\"> \r\n\t\t\t\t<li class=\"cash_li li1 load\"> \r\n\t\t\t\t\t<a href=\"https://game.naver.com/cash/myGameCash.nhn\" target=\"_blank\">\r\n\t\t\t\t\t\t<span class=\"ico\">캐쉬 :</span> \r\n\t\t\t\t\t\t<em id=\"gamecash\" class=\"cnt\">0</em><span class=\"text\">원</span>\r\n\t\t\t\t\t\t<span class=\"tip\"><em>tip: </em>내 게임캐쉬</span>\r\n\t\t\t\t\t</a>\r\n\t\t\t\t</li>\r\n\t\t\t\t<li class=\"cash_li li3 load\">\r\n\t\t\t\t\t<a href=\"https://game.naver.com/user/myCoupon.nhn\" target=\"_blank\">\r\n\t\t\t\t\t\t<span class=\"ico\">쿠폰 :</span>\r\n\t\t\t\t\t\t<em id=\"coupon\" class=\"cnt\">0</em><span class=\"text\">장</span>\r\n\t\t\t\t\t\t<span class=\"tip\"><em>tip: </em>내 쿠폰</span>\r\n\t\t\t\t\t</a> \r\n\t\t\t\t</li>\r\n\t\t\t</ul>\r\n        </div>\r\n        \r\n        \r\n        </div>\r\n    </div>\r\n    </div>\r\n    <!-- 프로모션 레이어 -->\r\n    <div class=\"global_promo\" style=\"display:none;\">\r\n    \t<h2 class=\"blind\">프로모션</h2>\r\n    \t<div class=\"promo_inr\">\r\n        \t<div class=\"promo_wrap\" >\r\n            \t<ul>\r\n                \r\n            \t\r\n\t                <li>\r\n\t\t\t\t\t\t\r\n\t\t\t\t\t\t\r\n\t\t\t\t\t\t\r\n\t\t                \t<img src=\"https://navergame.phinf.naver.net/20170517_182/1494999221917WPOHa_JPEG/07_TodaysPick_702x230.jpg?type=ori\" width=\"702\" height=\"230\" alt=\"개발자 블록맵 콘텐스트\">\r\n\t\t\t\t\t\t\r\n\t\t\t\t\t\t\r\n\t                    <dl>\r\n\t                    <dt class=\"tit\"><strong>개발자 블록맵 콘텐스트</strong><a href=\"https://tr.playnetwork.co.kr/event/2017/05/01_devblockmap/event.asp\" class=\"N=a:prl.title\">테일즈런너</a></dt>\r\n\t\t\t\t\t\t<dd class=\"dsc\">테런 개발자들의 불꽃튀기는 접전<br> \r\n최고의 맵을 골라라!</dd>\r\n\t                    </dl>\r\n\t                    \r\n\t                    \r\n\t                    <a href=\"javascript:;\" class=\"lnk_play N=a:prl.start\" onclick=\"gdp.jslib.goMirror('P_TR', 'ONG'); return false;\"><img src=\"https://images.playnetwork.co.kr/nvgame/img/blank.png\" width=\"140\" height=\"35\" alt=\"게임하러가기\"></a>\r\n\t                    <a href=\"https://tr.playnetwork.co.kr/event/2017/05/01_devblockmap/event.asp\" class=\"lnk_img\"><img src=\"https://images.playnetwork.co.kr/nvgame/img/blank.png\" width=\"360\" height=\"230\" alt=\"게임 바로가기\"></a>\r\n\t                </li>\r\n            \t\r\n\t                <li>\r\n\t\t\t\t\t\t\r\n\t\t\t\t\t\t\r\n\t\t\t\t\t\t\r\n\t\t                \t<img src=\"https://navergame.phinf.naver.net/20170116_212/1484544688187QGmwG_JPEG/%B4%D9%C5%A9%BF%A1%B5%A7%BF%C0%B8%AE%C1%F8_702x230_2.jpg?type=ori\" width=\"702\" height=\"230\" alt=\"죽이는 MMORPG의 귀환\">\r\n\t\t\t\t\t\t\r\n\t\t\t\t\t\t\r\n\t                    <dl>\r\n\t                    <dt class=\"tit\"><strong>죽이는 MMORPG의 귀환</strong><a href=\"https://origin.darkeden.game.naver.com/?view=ok\" class=\"N=a:prl.title\">다크에덴 : 오리진</a></dt>\r\n\t\t\t\t\t\t<dd class=\"dsc\">드디어 역사가 시작되었다!<br> \r\n다크에덴 오리진 정식오픈!</dd>\r\n\t                    </dl>\r\n\t                    \r\n\t                    \r\n\t                    <a href=\"javascript:;\" class=\"lnk_play N=a:prl.start\" onclick=\"gdp.jslib.goMirror('P_PN027970', 'ONG'); return false;\"><img src=\"https://images.playnetwork.co.kr/nvgame/img/blank.png\" width=\"140\" height=\"35\" alt=\"게임하러가기\"></a>\r\n\t                    <a href=\"https://origin.darkeden.game.naver.com/?view=ok\" class=\"lnk_img\"><img src=\"https://images.playnetwork.co.kr/nvgame/img/blank.png\" width=\"360\" height=\"230\" alt=\"게임 바로가기\"></a>\r\n\t                </li>\r\n            \t\r\n\t                <li>\r\n\t\t\t\t\t\t\r\n\t\t\t\t\t\t\r\n\t\t\t\t\t\t\r\n\t\t                \t<img src=\"https://navergame.phinf.naver.net/20170605_101/1496631033686VI7wQ_JPEG/%BE%C6%C0%CC%BE%F0%BB%E7%C0%CC%C6%AE_%C3%DF%C3%B5%C0%CC%BA%A5%C6%AE_702x230.jpg?type=ori\" width=\"702\" height=\"230\" alt=\"신개념 FPS 게임!\">\r\n\t\t\t\t\t\t\r\n\t\t\t\t\t\t\r\n\t                    <dl>\r\n\t                    <dt class=\"tit\"><strong>신개념 FPS 게임!</strong><a href=\"https://tr.playnetwork.co.kr/event/2016/12/14_Seventeen/index.asp\" class=\"N=a:prl.title\">아이언사이트</a></dt>\r\n\t\t\t\t\t\t<dd class=\"dsc\">드론 전투! 신개념 FPS게임<br>\r\n아이언사이트 GRAND OPEN!\r\n\r\n</dd>\r\n\t                    </dl>\r\n\t                    \r\n\t                    \r\n\t                    <a href=\"javascript:;\" class=\"lnk_play N=a:prl.start\" onclick=\"gdp.jslib.goMirror('P_PN026899', 'ONG'); return false;\"><img src=\"https://images.playnetwork.co.kr/nvgame/img/blank.png\" width=\"140\" height=\"35\" alt=\"게임하러가기\"></a>\r\n\t                    <a href=\"https://tr.playnetwork.co.kr/event/2016/12/14_Seventeen/index.asp\" class=\"lnk_img\"><img src=\"https://images.playnetwork.co.kr/nvgame/img/blank.png\" width=\"360\" height=\"230\" alt=\"게임 바로가기\"></a>\r\n\t                </li>\r\n            \t\r\n\t                <li>\r\n\t\t\t\t\t\t\r\n\t\t\t\t\t\t\r\n\t\t\t\t\t\t\r\n\t\t                \t<img src=\"https://navergame.phinf.naver.net/20170227_164/1488158926317Ef1l7_JPEG/%C5%F5%B5%A5%C0%CC%C7%C8_702x230.jpg?type=ori\" width=\"702\" height=\"230\" alt=\"액션쾌감!!!\">\r\n\t\t\t\t\t\t\r\n\t\t\t\t\t\t\r\n\t                    <dl>\r\n\t                    <dt class=\"tit\"><strong>액션쾌감!!!</strong><a href=\"https://df.game.naver.com/df/pg/wpriest2nd?intro=yes\" class=\"N=a:prl.title\">던전앤파이터</a></dt>\r\n\t\t\t\t\t\t<dd class=\"dsc\">전 세계 5억명이 선택한 최고의 액션RPG </dd>\r\n\t                    </dl>\r\n\t                    \r\n\t                    \r\n\t                    <a href=\"javascript:;\" class=\"lnk_play N=a:prl.start\" onclick=\"gdp.jslib.goMirror('P_PN001085', 'ONG'); return false;\"><img src=\"https://images.playnetwork.co.kr/nvgame/img/blank.png\" width=\"140\" height=\"35\" alt=\"게임하러가기\"></a>\r\n\t                    <a href=\"https://df.game.naver.com/df/pg/wpriest2nd?intro=yes\" class=\"lnk_img\"><img src=\"https://images.playnetwork.co.kr/nvgame/img/blank.png\" width=\"360\" height=\"230\" alt=\"게임 바로가기\"></a>\r\n\t                </li>\r\n            \t\r\n\t                <li>\r\n\t\t\t\t\t\t\r\n\t\t\t\t\t\t\r\n\t\t\t\t\t\t\r\n\t\t                \t<img src=\"https://navergame.phinf.naver.net/20170202_282/14860140029926uNXw_JPEG/%C5%AC%B7%CE%C0%FA%BD%BA_702x230.jpg?type=ori\" width=\"702\" height=\"230\" alt=\"클로저스\">\r\n\t\t\t\t\t\t\r\n\t\t\t\t\t\t\r\n\t                    <dl>\r\n\t                    <dt class=\"tit\"><strong>클로저스</strong><a href=\"https://closers.nexon.game.naver.com/events/2017/02/02/OpenEvent.aspx\" class=\"N=a:prl.title\">100만명이 선택한 RPG</a></dt>\r\n\t\t\t\t\t\t<dd class=\"dsc\">클로저스 X NAVER<br> \r\n이제 네이버를 통해서도 클로저스를 즐길실 수 있습니다.</dd>\r\n\t                    </dl>\r\n\t                    \r\n\t                    \r\n\t                    <a href=\"javascript:;\" class=\"lnk_play N=a:prl.start\" onclick=\"gdp.jslib.goMirror('P_PN025806', 'ONG'); return false;\"><img src=\"https://images.playnetwork.co.kr/nvgame/img/blank.png\" width=\"140\" height=\"35\" alt=\"게임하러가기\"></a>\r\n\t                    <a href=\"https://closers.nexon.game.naver.com/events/2017/02/02/OpenEvent.aspx\" class=\"lnk_img\"><img src=\"https://images.playnetwork.co.kr/nvgame/img/blank.png\" width=\"360\" height=\"230\" alt=\"게임 바로가기\"></a>\r\n\t                </li>\r\n            \t\r\n                </ul>\r\n            </div>\r\n            <div class=\"pagination_bnr\">\r\n\t            \r\n\t            \t<a href=\"javascript:;\" class=\"on\" class=\"N=a:prl.page\">1</a>                    \r\n\t            \r\n\t            \t<a href=\"javascript:;\"  class=\"N=a:prl.page\">2</a>                    \r\n\t            \r\n\t            \t<a href=\"javascript:;\"  class=\"N=a:prl.page\">3</a>                    \r\n\t            \r\n\t            \t<a href=\"javascript:;\"  class=\"N=a:prl.page\">4</a>                    \r\n\t            \r\n\t            \t<a href=\"javascript:;\"  class=\"N=a:prl.page\">5</a>                    \r\n\t            \r\n            </div>\r\n            \r\n            <a href=\"javascript:;\" class=\"pro_prev N=a:prl.prev\">프로모션 이전페이지</a>\r\n        \t<a href=\"javascript:;\" class=\"pro_next N=a:prl.next\">프로모션 다음페이지</a>\t\r\n        </div>\r\n    </div>\r\n    <!-- //프로모션 레이어 -->\r\n\r\n</div>\r\n\r\n\r\n<script type=\"text/javascript\">\r\n\r\njQuery(function(){\r\n\tjQuery('head').append('<meta http-equiv=\"X-UA-Compatible\" content=\"IE=edge\">');\t\t\t\r\n\t\r\n\tjQuery.ajax({\r\n\t\turl : jQuery('#wwwUrl').val() + '/gnbMyInfo.nhn',\r\n\t\ttype : 'get',\r\n\t\tcache : false,\r\n\t\theaders : {\"cache-control\":\"no-cache\",\"pragma\":\"no-cache\"},\r\n\t\tdataType : \"jsonp\",\r\n\t    jsonp : \"callback\",\r\n\t\tsuccess : function(result) {\r\n\t\t\t//게임캐쉬\r\n\t\t\tjQuery('#gamecash').parent().parent().removeClass('load');\r\n\t\t\tjQuery('#gamecash').html(result.gameCash);\r\n\t\t\t\r\n\t\t\t//쿠폰\r\n\t\t\tjQuery('#coupon').parent().parent().removeClass('load');\r\n\t\t\tjQuery('#coupon').html(result.coupon);\r\n\t\t}\r\n\t});\r\n});\t\r\n\r\nvar gameId = \"P_PN025542\";\r\njQuery(function(){\r\n\tjQuery.ajax({\r\n        url : jQuery('#wwwUrl').val() + '/ajax/couponGnbBanner.nhn',\r\n        type : 'get',\r\n        cache : false,\r\n        headers : {\"cache-control\":\"no-cache\",\"pragma\":\"no-cache\"},\r\n        dataType : \"jsonp\",\r\n        jsonp : \"callback\",\r\n        success : function(result) {\r\n            // 표시 쿠폰할 쿠폰이 있다면\r\n            if(result && result.showGnbCouponBanner == true){\r\n                jQuery('#gnbCouponBannerLink').html(decodeURIComponent( (result.gnbTopMessage).replace(/\\+/g, \"%20\")));\r\n            \tjQuery('#gnbCouponBannerLink').attr(\"href\", result.gnbTopLink);\r\n            \tjQuery('#gnbCouponBannerContainer').show();\r\n                if(result.popupUseYn == \"Y\" \r\n                \t\t&& getCookie(\"COUPON_EVENT_POPUP_HIDE\") != 'Y'\r\n                \t\t&& jQuery.inArray(gameId, result.gameIdList) > -1 ){\r\n                \topenCouponBanner(result.couponEventNo);\r\n                }\r\n\r\n                //[START]temp method for fifa online3 event\r\n                if(result.popupUseYn == \"Y\"\r\n            \t\t&& getCookie(\"COUPON_EVENT_POPUP_HIDE\") != 'Y'\r\n                    && result.gameIdList.length == 0  ){\r\n                    var popupBlockList = [''];\r\n                \tif(jQuery.inArray(gameId, popupBlockList) > -1){\r\n\t\t\t\t\t\t;\r\n                \t} else {\r\n                    \topenTeaserPopup();\r\n                \t}\r\n\t            }\r\n                //[END]temp method for fifa online3 event            \r\n            }\r\n        }\r\n    });\r\n});\r\nfunction openCouponBanner(couponEventNo){\r\n\tvar wOption = 'toolbar=0,menubar=0,resizable=yes,scrollbars=no,width=' + 300 + ',height=' + 315 + \",status=no\";\r\n\twindow.open(jQuery('#wwwUrl').val() + \"/coupon/popup/downloadEvent.nhn?couponEventNo=\" + couponEventNo, 'couponEventPopup', wOption);\r\n}\r\nfunction getCookie(CookieName){\r\n    var CookieVal = null;\r\n    if(document.cookie){\r\n        var arr = document.cookie.split((escape(CookieName) + '=')); \r\n        if(arr.length >= 2){\r\n            var arr2 = arr[1].split(';');\r\n            CookieVal  = unescape(arr2[0]);\r\n        }\r\n    }\r\n    return CookieVal;\r\n}\r\n\r\n//[START]temp method for fifa online3 event\r\nfunction openTeaserPopup(){\r\n    var centerLeft = parseInt(window.innerWidth || document.documentElement.clientWidth || document.body.clientWidth)/2;\r\n    var centerTop = parseInt(window.innerHeight || document.documentElement.clientHeight || document.body.clientHeight)/2;\t\r\n\t\r\n    centerLeft = Math.floor(centerLeft) - 150;\r\n    centerTop = Math.floor(centerTop) - 157;\r\n    \r\n    var wOption = 'toolbar=0,menubar=0,resizable=yes,scrollbars=no,width=' + 300 + ',height=' + 315 + \",status=no\";\r\n    wOption += (',top=' + centerTop + ',left=' + centerLeft);\r\n    window.open(\"https://game.naver.com/coupon/popup/teaserEvent.nhn\", 'teaserEventPopup', wOption);\r\n}\r\n//[END]temp method for fifa online3 event\r\n\r\n\r\n</script>\r\n\r\n"},
    
    errorMsg: '',
    destUrl: '',
    
    isQA : 'false'
});

// Do not delete this comment. tts monitoring.
// GDP_ABCDEFGHIJKLMNOPQRSTUVWXYZ