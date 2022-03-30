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