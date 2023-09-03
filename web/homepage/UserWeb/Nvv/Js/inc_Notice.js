function fnNoticeList() {

	layer.open({
		type: 2,
		title: ' ',
		time: 0,
		shadeClose: true,
		shade: 0.8,
		area: ['530px', '522px'],
		content: '/Notice/LayerNtcList',
	});

}

function fnNoticeView(nNum,nNtcNum) {

	layer.open({
		type: 2,
		title: ' ',
		time: 0,
		shadeClose: true,
		shade: 0.8,
		area: ['530px', '522px'],
		content: '/Notice/NtcView?nNtcId=' + nNtcNum + "&nNum=" + nNum,
	});

}
