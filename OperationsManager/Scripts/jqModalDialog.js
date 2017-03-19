var $dialog = null;

jQuery.showModalDialog = function (options) {

    var defaultOptns = {
        url: null,
        dialogArguments: null,
        height: 'auto',
        width: 'auto',
        position: 'center',
        resizable: true,
        scrollable: true,
        onClose: function () { },
        returnValue: null,
        doPostBackAfterCloseCallback: false,
        postBackElementId: null
    };

    var fns = {
        close: function () {
            opts.returnValue = $dialog.returnValue;
            $dialog = null;
            opts.onClose();
            if (opts.doPostBackAfterCloseCallback) {
                postBackForm(opts.postBackElementId);
            }
        },
        adjustWidth: function () { $frame.css("width", "100%"); }
    };

    // build main options before element iteration

    var opts = $.extend({}, defaultOptns, options);

    var $frame = $('<iframe id="iframeDialog" />');

    if (opts.scrollable)
        $frame.css('overflow', 'auto');

    $frame.css({
        'padding': 0,
        'margin': 0,
        'padding-bottom': 10
    });

    var $dialogWindow = $frame.dialog({
        autoOpen: true,
        modal: true,
        width: opts.width,
        height: opts.height,
        resizable: opts.resizable,
        position: opts.position,
        overlay: {
            opacity: 0.5,
            background: "black"
        },
        close: fns.close,
        resizeStop: fns.adjustWidth
    });

    $frame.attr('src', opts.url);
    fns.adjustWidth();

    $frame.load(function () {
        if ($dialogWindow) {

            var maxTitleLength = 50;
            var title = $(this).contents().find("title").html();

            if (title.length > maxTitleLength) {
                title = title.substring(0, maxTitleLength) + '...';
            }
            $dialogWindow.dialog('option', 'title', title);
        }
    });

    $dialog = new Object();
    $dialog.dialogArguments = opts.dialogArguments;
    $dialog.dialogWindow = $dialogWindow;
    $dialog.returnValue = null;
}

function postBackForm(targetElementId) {
    var theform;
    theform = document.forms[0];
    theform.__EVENTTARGET.value = targetElementId;
    theform.__EVENTARGUMENT.value = "";
    theform.submit();
}

//**** Following code to be entered on the pages which will be opened as a dialog ****

//var prntWindow = getParentWindowWithDialog(); //$(top)[0];

//var $dlg = prntWindow && prntWindow.$dialog;

//function getParentWindowWithDialog() {
//    var p = window.parent;
//    var previousParent = p;
//    while (p != null) {
//        if ($(p.document).find('#iframeDialog').length) return p;

//        p = p.parent;

//        if (previousParent == p) return null;

//        // save previous parent

//        previousParent = p;
//    }
//    return null;
//}

//function setWindowReturnValue(value) {
//    if ($dlg) $dlg.returnValue = value;
//    window.returnValue = value; // in case popup is called using showModalDialog

//}

//function getWindowReturnValue() {
//    // in case popup is called using showModalDialog

//    if (!$dlg && window.returnValue != null)
//        return window.returnValue;

//    return $dlg && $dlg.returnValue;
//}

//if ($dlg) window.dialogArguments = $dlg.dialogArguments;
//if ($dlg) window.close = function () { if ($dlg) $dlg.dialogWindow.dialog('close'); };


//**** Following code will be used to open the dialog ****//
//var url = 'test.html';

//$.showModalDialog({
//    url: url,
//    height: 500,
//    width: 900,
//    scrollable: false,
//    onClose: function () { var returnedValue = this.returnValue; }
//});

//use $dlg.* instead of window.*