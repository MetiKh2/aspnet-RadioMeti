function ShowMessage(title, text, theme) {
    window.createNotification({
        closeOnClick: true,
        displayCloseButton: false,
        positionClass: 'nfc-bottom-right',
        showDuration: 4000,
        theme: theme !== '' ? theme : 'success'
    })({
        title: title !== '' ? title : 'Notification',
        message: decodeURI(text)
    });
}

function FillPageId(pageId) {
    $('#PageId').val(pageId)
    $('#filter-form').submit()
}
$('.searchable-select').dropdown({
    limitCount: Infinity,
    input: '<input type="text" maxLength="20" placeholder="Search">',
});
//(function () {
//	//var availableTags = [
//	//	"ActionScript",
//	//	"AppleScript",
//	//	"Asp",
//	//	"BASIC",
//	//	"C",
//	//	"C++",
//	//	"Clojure",
//	//	"COBOL",
//	//	"ColdFusion",
//	//	"Erlang",
//	//	"Fortran",
//	//	"Groovy",
//	//	"Haskell",
//	//	"Java",
//	//	"JavaScript",
//	//	"Lisp",
//	//	"Perl",
//	//	"PHP",
//	//	"Python",
//	//	"Ruby",
//	//	"Scala",
//	//	"Scheme"
//	//];
//	//$(".autocomplete").autocomplete({
//	//	source: availableTags
//	//});
	
//	//$(".autocomplete").autocomplete("option", "source", ["c++", "java", "php", "coldfusion", "javascript", "asp", "ruby"]);
//})
	
