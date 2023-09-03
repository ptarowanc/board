/* 사용 레이어 적용 소스

	// 퀵메뉴 세팅
	var cClass1 = new acQuickMove();
	cClass1.defaultTop = 100;				// 퀵메뉴 기본 top 좌표
	cClass1.objName = "LQuickMenu1";		// 퀵메뉴 id
	cClass1.speed = 2						// 스크롤 속도 1 이상

	cClass1.alfControlQuick("cClass1");		// 퀵메뉴 스크롤 함수 호출

*/
var acQuickMove = function()
{
	this.defaultTop = 100;			// 기본 top 좌표
	this.objName = "";				// 대상 객체 id
	this.speed = 4;					// 이동속도 1 이상
	this.moveFlag = 1;				// 스크롤 여부 1:움직임 / 0:안움직임
	this.moveTop = this.defaultTop;	// 브라우저 Top 기준 이동시작 좌표
	this.initFlag = 0;		// 초기화 여부(고정값)

	this.alfControlQuick = function(CURClsName)
	{
		var CURClass = eval(CURClsName);
		if (!CURClass.moveFlag)
		{
			setTimeout(CURClsName + ".alfControlQuick('"+CURClsName+"')", 10);
			return;
		}

		var obj = document.getElementById(CURClass.objName);
		if (!obj)
		{
			alert("레이어 객체가 존재하지 않습니다. objName을 확인해주세요");
			return;
		}
		
		if (CURClass.initFlag == "0")
		{
			obj.style.top = CURClass.defaultTop + "px";
			CURClass.initFlag = "1";
		}
		var objTop = Number(obj.style.top.replace("px", ""));
		//var top = (document.documentElement.scrollTop || document.body.scrollTop);
		var top = acfGetScrollTop();

		if (parseInt(objTop) < parseInt((CURClass.defaultTop + top - (CURClass.defaultTop - CURClass.moveTop))))
		{
			if ((CURClass.defaultTop + top - (CURClass.defaultTop - CURClass.moveTop)) - objTop > 0)
			{
				gab = ((CURClass.defaultTop + top - (CURClass.defaultTop - CURClass.moveTop)) - objTop)/ (CURClass.speed*2);
				gab = (gab < 1) ? 1 : gab;
				objTop += gab;
				obj.style.top = objTop + "px";

				// 테스트용 좌표 출력 코드
				//obj.innerHTML = "Down<br />default Top : " + CURClass.defaultTop + "<br />objTop : " + parseInt(objTop) + "<br />scrolltop : " + top + "<br />speed : " + CURClass.speed;
			}
		}
		else if (parseInt(objTop) > parseInt(CURClass.moveTop + top))
		{
			if ((CURClass.moveTop + top) - objTop < 0 && objTop > CURClass.defaultTop)
			{
				gab = ((CURClass.moveTop + top) - objTop)/ (CURClass.speed*2);
				gab = (gab > -1) ? -1 : gab;
				objTop += gab;

				if (objTop < CURClass.defaultTop)
				{
					objTop = CURClass.defaultTop;
				}

				obj.style.top = objTop + "px";

				// 테스트용 좌표 출력 코드
				//obj.innerHTML = "Up<br />default Top : " + CURClass.defaultTop + "<br />objTop : " + parseInt(objTop) + "<br />scrolltop : " + top + "<br />speed : " + CURClass.speed;
			}
			else if (objTop > CURClass.defaultTop)
			{
				obj.style.top = CURClass.defaultTop + "px";
			}
		}

		setTimeout(CURClsName + ".alfControlQuick('"+CURClsName+"')", 10);
		return;
	}
}

function acfGetScrollTop()
{
	if (window.pageYOffset)
	{     
		scrollTop = window.pageYOffset 
	} 
	else if(document.documentElement && document.documentElement.scrollTop)
	{    
		scrollTop = document.documentElement.scrollTop; 
	} 
	else if(document.body)
	{
		scrollTop = document.body.scrollTop; 
	}    

	if(window.pageXOffset)
	{     
		scrollLeft=window.pageXOffset 
	} 
	else if(document.documentElement && document.documentElement.scrollLeft)
	{    
		scrollLeft=document.documentElement.scrollLeft; 
	}
	else if(document.body)
	{ 
		scrollLeft=document.body.scrollLeft; 
	}

	return scrollTop;
}