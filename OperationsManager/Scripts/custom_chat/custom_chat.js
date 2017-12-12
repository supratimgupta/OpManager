var openedChatWindows = [];
$(document).on('click', '.panel-heading span.icon_minim', function (e) {
    var $this = $(this);
    if (!$this.hasClass('panel-collapsed')) {
        $this.parents('.panel').find('.panel-body').slideUp();
        $this.addClass('panel-collapsed');
        $this.removeClass('glyphicon-minus').addClass('glyphicon-plus');
    } else {
        $this.parents('.panel').find('.panel-body').slideDown();
        $this.removeClass('panel-collapsed');
        $this.removeClass('glyphicon-plus').addClass('glyphicon-minus');
    }
});
//$(document).on('focus', '.panel-footer input.chat_input', function (e) {
//    var $this = $(this);
//    //var mainDiv = 
//    if ($('#minim_chat_window').hasClass('panel-collapsed')) {
//        $this.parents('.panel').find('.panel-body').slideDown();
//        $('#minim_chat_window').removeClass('panel-collapsed');
//        $('#minim_chat_window').removeClass('glyphicon-plus').addClass('glyphicon-minus');
//    }
//});
//$(document).on('click', '.new_chat', function (e) {
//    debugger;
//    var lastChild = $(".chat-container div:last-child");
//    var size = $(".chat-window:last-child").css("margin-left");
//    alert(size);
//    size_total = parseInt(size) + 400;
//    if (isNaN(size_total))
//    {
//        size_total = 10;
//    }
//    alert(size_total);

//    var newChatWindow = createChatWindow();
//    var clone = $(newChatWindow).appendTo(".chat-container");
//    openedChatWindows.push(newChatWindow);
//    clone.css("margin-left", size_total);
//    $(".msg_container_base").scrollTop($(".msg_container_base")[0].scrollHeight);
//});

function openChatPupup(uRowId, uName)
{
    
    var lastChild = $(".chat-container div:last-child");
    var size = $(".chat-window:last-child").css("margin-left");
    //alert(size);
    size_total = parseInt(size) + 400;
    if (isNaN(size_total)) {
        size_total = 10;
    }
    //alert(size_total);

    $.ajax({
        type: 'GET',
        url: '../../Notification/Chat/GetChatHistory?sender=' + uRowId + '&skipRows=0',
        dataType: "json",
        success: function (resultData) {
            
            var newChatWindow = createChatWindow(uRowId, uName, resultData);
            if (newChatWindow != null)
            {
                var clone = $(newChatWindow).appendTo(".chat-container");
                openedChatWindows.push(uRowId);
                clone.css("margin-left", size_total);
                $(".msg_container_base").scrollTop($(".msg_container_base")[0].scrollHeight);
            }         

            makeMsgsReadOnServer(uRowId);
        },
        error: function (error) { alert(error); }
    });

}

$(document).on('click', '.icon_close', function (e) {
    $(this).parent().parent().parent().parent().parent().remove();
    //$("#chat_window_1").remove();
    //var arrAllChatWindows = document.getElementsByClassName("chat-window");

    //for(i=0;i<arrAllChatWindows.length;i++)
    //{
    //    var leftMargin = $(arrAllChatWindows[i]).css("margin-left");
    //    var leftMarginInt = parseInt(leftMargin);

    //    if (leftMarginInt > 400)
    //    {
    //        leftMarginInt = leftMarginInt - 400;
    //    }

    //    $(arrAllChatWindows[i]).css("margin-left", leftMarginInt+"px");
    //}
    openedChatWindows.splice(2, 1);
});

function recieveMessageHandler(sender, reciever, messageList, chatLogId)
{
    
    var chatBoxId = "chat_window_" + sender.UserRowId;

    if(document.getElementById(chatBoxId))
    {
        var divMsgContainer = document.getElementById(chatBoxId).getElementsByClassName("msg_container_base")[0];
        divMsgContainer.innerHTML = "";

        //Trial code - Not final
        $(divMsgContainer).attr('onscroll', 'scrollHandler(' + sender.UserRowId + ', "' + sender.UserName + '",this);');

        var inputScrollIndex = document.createElement("input");
        inputScrollIndex.type = "hidden";
        inputScrollIndex.value = "0";
        inputScrollIndex.className = "scrollIndex";

        divMsgContainer.appendChild(inputScrollIndex);
        //Trial code - Not final

        if (messageList) {
            for (i = 0; i < messageList.length; i++) {
                var messageHolder = document.createElement("div");
                var divAvatar = document.createElement("div");
                var imgAvatar = document.createElement("img");
                imgAvatar.src = "http://www.bitrebels.com/wp-content/uploads/2011/02/Original-Facebook-Geek-Profile-Avatar-1.jpg";
                imgAvatar.className = "img-responsive chat-img";
                var iAmSender = "true";
                divAvatar.appendChild(imgAvatar);
                divAvatar.className = "col-md-2 chat-col-md-2 col-xs-2 avatar";

                var divMsgWidth = document.createElement("div");

                var divActualMsg = document.createElement("div");

                var pActualMsg = document.createElement("p");
                pActualMsg.innerHTML = messageList[i].Message;

                var timeMsgTime = document.createElement("time");
                timeMsgTime.innerHTML = messageList[i].MessageSentTimesAgo;

                divActualMsg.appendChild(pActualMsg);
                divActualMsg.appendChild(timeMsgTime);
                divMsgWidth.appendChild(divActualMsg);

                messageHolder.appendChild(divAvatar);
                messageHolder.appendChild(divMsgWidth);

                if (sender.UserRowId === messageList[i].SentByUser.UserMasterId) {
                    iAmSender = "false";
                }

                if (iAmSender === "false") {
                    messageHolder.className = "row msg_container base_receive";
                    divMsgWidth.className = "col-xs-10 col-md-10";
                    divActualMsg.className = "messages msg_receive";

                    messageHolder.appendChild(divAvatar);
                    messageHolder.appendChild(divMsgWidth);
                }
                else {
                    messageHolder.className = "row msg_container base_sent";
                    divMsgWidth.className = "col-md-10 col-xs-10 chat-col-md-10";
                    divActualMsg.className = "messages msg_sent";

                    messageHolder.appendChild(divMsgWidth);
                    messageHolder.appendChild(divAvatar);

                }

                divMsgContainer.appendChild(messageHolder);
            }
        }

        sendAcknowledgement(sender.UserRowId, reciever.UserRowId);
    }
    //else
    //{
    //    createChatWindow(sender.UserRowId, sender.UserName, messageList);
    //    sendAcknowledgement(sender.UserRowId, reciever.UserRowId);
    //}
}

function sendAcknowledgement(senderId, recieverId)
{
    
    var notificationHub = $.connection.notificationHub;
    //var currentUser = @session.UserMasterId;
    notificationHub.server.acknowledgeRead(senderId, recieverId);
}

function createChatWindow(uRowId, uName, messageList)
{
    
    if(openedChatWindows.length<3)
    {
        if (document.getElementById("chat_window_" + uRowId))
        {
            alert('Chat box with ' + uName + ' is already open.');
            return null;
        }
        var divParent = document.createElement("div");
        divParent.className = "row chat-window col-xs-5 col-md-3";
        divParent.id = "chat_window_" + uRowId;
        $(divParent).attr('style', 'margin-left:10px;');

        var divFirstChild = document.createElement("div");
        divFirstChild.className = "col-xs-12 col-md-12";

        var divChatPanel = document.createElement("div");
        divChatPanel.className = "panel chat-panel panel-default";

        var divChatHeader = document.createElement("div");
        divChatHeader.className = "panel-heading top-bar chat-top-bar";

        var divChatterName = document.createElement("div");
        divChatterName.className = "col-md-8 col-xs-8";

        var h3ChatterName = document.createElement("h3");
        h3ChatterName.className = "panel-title";

        var spanCommentGlyphicon = document.createElement("span");
        spanCommentGlyphicon.className = "glyphicon glyphicon-comment";

        h3ChatterName.innerHTML = spanCommentGlyphicon.innerHTML + uName;

        var divChatBoxIcons = document.createElement("div");
        divChatBoxIcons.className = "col-md-4 col-xs-4";
        $(divChatBoxIcons).attr('style', 'text-align: right;');

        var aMinimumWindow = document.createElement("a");
        aMinimumWindow.href = "#";
        
        var spanMinimumWindow = document.createElement("span");
        spanMinimumWindow.id = "minim_chat_window";
        spanMinimumWindow.className = "glyphicon glyphicon-minus icon_minim";

        var aCloseWindow = document.createElement("a");
        aCloseWindow.href = "#";

        var spanCloseWindow = document.createElement("span");
        spanCloseWindow.className = "glyphicon glyphicon-remove icon_close";
        $(spanCloseWindow).attr('data-id', "chat_window_" + uRowId);

        var divMsgContainer = document.createElement("div");
        divMsgContainer.className = "panel-body msg_container_base";

        //Trial code - Not final
        $(divMsgContainer).attr('onscroll', 'scrollHandler(' + uRowId + ', "'+uName+'", this);');

        var inputScrollIndex = document.createElement("input");
        inputScrollIndex.type = "hidden";
        inputScrollIndex.value = "0";
        inputScrollIndex.className = "scrollIndex";

        divMsgContainer.appendChild(inputScrollIndex);
        //Trial code - Not final

        if (messageList)
        {
            for (i = 0; i < messageList.length; i++) {
                var messageHolder = document.createElement("div");
                var divAvatar = document.createElement("div");
                var imgAvatar = document.createElement("img");
                imgAvatar.src = "http://www.bitrebels.com/wp-content/uploads/2011/02/Original-Facebook-Geek-Profile-Avatar-1.jpg";
                imgAvatar.className = "img-responsive chat-img";
                var iAmSender = "true";
                divAvatar.appendChild(imgAvatar);
                divAvatar.className = "col-md-2 chat-col-md-2 col-xs-2 avatar";

                var divMsgWidth = document.createElement("div");

                var divActualMsg = document.createElement("div");

                var pActualMsg = document.createElement("p");
                pActualMsg.innerHTML = messageList[i].Message;

                var timeMsgTime = document.createElement("time");
                timeMsgTime.innerHTML = messageList[i].MessageSentTimesAgo;

                divActualMsg.appendChild(pActualMsg);
                divActualMsg.appendChild(timeMsgTime);
                divMsgWidth.appendChild(divActualMsg);

                messageHolder.appendChild(divAvatar);
                messageHolder.appendChild(divMsgWidth);

                if(uRowId===messageList[i].SentByUser.UserMasterId)
                {
                    iAmSender = "false";
                }

                if(iAmSender==="false")
                {
                    messageHolder.className = "row msg_container base_receive";
                    divMsgWidth.className = "col-xs-10 col-md-10";
                    divActualMsg.className = "messages msg_receive";

                    messageHolder.appendChild(divAvatar);
                    messageHolder.appendChild(divMsgWidth);
                }
                else
                {
                    messageHolder.className = "row msg_container base_sent";
                    divMsgWidth.className = "col-md-10 col-xs-10 chat-col-md-10";
                    divActualMsg.className = "messages msg_sent";

                    messageHolder.appendChild(divMsgWidth);
                    messageHolder.appendChild(divAvatar);
                    
                }

                divMsgContainer.appendChild(messageHolder);
            }
        }
        

        var divFooter = document.createElement("div");
        divFooter.className = "panel-footer";

        var divInputGroup = document.createElement("div");
        divInputGroup.className = "input-group";

        var txtMessage = document.createElement("input");
        txtMessage.type = "text";
        txtMessage.className = "form-control input-sm chat_input";
        txtMessage.placeholder = "Write your message here...";

        var spanBtnGroup = document.createElement("span");
        spanBtnGroup.className = "input-group-btn";

        var btnSend = document.createElement("button");
        btnSend.className = "btn btn-primary btn-sm";
        btnSend.innerHTML = "Send";

        $(btnSend).attr('onclick', 'sendMessage('+uRowId+',this)');

        //Adding controls together

        spanBtnGroup.appendChild(btnSend);
        divInputGroup.appendChild(txtMessage);
        divInputGroup.appendChild(spanBtnGroup);

        divFooter.appendChild(divInputGroup);

        aMinimumWindow.appendChild(spanMinimumWindow);
        aCloseWindow.appendChild(spanCloseWindow);

        divChatBoxIcons.appendChild(aMinimumWindow);
        divChatBoxIcons.appendChild(aCloseWindow);

        divChatterName.appendChild(h3ChatterName);

        divChatHeader.appendChild(divChatterName);
        divChatHeader.appendChild(divChatBoxIcons);

        divChatPanel.appendChild(divChatHeader);
        divChatPanel.appendChild(divMsgContainer);
        divChatPanel.appendChild(divFooter);

        divFirstChild.appendChild(divChatPanel);

        divParent.appendChild(divFirstChild);
        return divParent;
    }
    else
    {
        alert('Maximum of three chat windows can be opened, close one to open another');
    }
}

var ajax_pending = false;

function scrollHandler(uRowId, uName, msgContainer)
{
    if ($(msgContainer).scrollTop() == 0) {
        setTimeout(function () {
            // Simulate retrieving 4 messages
            
            if (ajax_pending)
            {
                return;
            }

            var scrollIndex = msgContainer.getElementsByClassName("scrollIndex")[0].value;
            scrollIndex = parseInt(scrollIndex) + 10;

            ajax_pending = true;

            $.ajax({
                type: 'GET',
                url: '../../Notification/Chat/GetChatHistory?sender=' + uRowId + '&skipRows=' + scrollIndex,
                dataType: "json",
                success: function (resultData) {
                    //debugger;

                    prependHistory(uRowId, uName, msgContainer, resultData);

                    msgContainer.getElementsByClassName("scrollIndex")[0].value = scrollIndex + 10;

                    ajax_pending = false;

                    $(msgContainer).scrollTop(30);
                },
                error: function (error) { alert(error); }
            });

            //for (var i = 0; i < 4; i++) {
            //    $(msgContainer).prepend('<div class="messages">Newly Loaded messages<br/><span class="date">' + Date() + '</span> </div>');
            //}
            // Hide loader on success
            //$('#loader').hide();
            // Reset scroll
            //$(msgContainer).scrollTop(30);
        }, 738);
    }
}

function prependHistory(uRowId, uName, msgContainer, messageList)
{
    if (messageList) {
        //debugger;
        var startingPoint = messageList.length - 1;
        for (i = startingPoint; i >= 0; i--) {
            var messageHolder = document.createElement("div");
            var divAvatar = document.createElement("div");
            var imgAvatar = document.createElement("img");
            imgAvatar.src = "http://www.bitrebels.com/wp-content/uploads/2011/02/Original-Facebook-Geek-Profile-Avatar-1.jpg";
            imgAvatar.className = "img-responsive chat-img";
            var iAmSender = "true";
            divAvatar.appendChild(imgAvatar);
            divAvatar.className = "col-md-2 chat-col-md-2 col-xs-2 avatar";

            var divMsgWidth = document.createElement("div");

            var divActualMsg = document.createElement("div");

            var pActualMsg = document.createElement("p");
            pActualMsg.innerHTML = messageList[i].Message;

            var timeMsgTime = document.createElement("time");
            timeMsgTime.innerHTML = messageList[i].MessageSentTimesAgo;

            divActualMsg.appendChild(pActualMsg);
            divActualMsg.appendChild(timeMsgTime);
            divMsgWidth.appendChild(divActualMsg);

            messageHolder.appendChild(divAvatar);
            messageHolder.appendChild(divMsgWidth);

            if (uRowId === messageList[i].SentByUser.UserMasterId) {
                iAmSender = "false";
            }

            if (iAmSender === "false") {
                messageHolder.className = "row msg_container base_receive";
                divMsgWidth.className = "col-xs-10 col-md-10";
                divActualMsg.className = "messages msg_receive";

                messageHolder.appendChild(divAvatar);
                messageHolder.appendChild(divMsgWidth);
            }
            else {
                messageHolder.className = "row msg_container base_sent";
                divMsgWidth.className = "col-md-10 col-xs-10 chat-col-md-10";
                divActualMsg.className = "messages msg_sent";

                messageHolder.appendChild(divMsgWidth);
                messageHolder.appendChild(divAvatar);

            }

            $(msgContainer).prepend(messageHolder.innerHTML);

            //divMsgContainer.appendChild(messageHolder);
        }
    }
}

// Assign scroll function to chatBox DIV
//$('.msg_container_base').scroll(function () {
//    if ($('.msg_container_base').scrollTop() == 0) {
//        // Display AJAX loader animation
//        //$('#loader').show();

//        // Youd do Something like this here
//        // Query the server and paginate results
//        // Then prepend
//        /*  $.ajax({
//              url:'getmessages.php',
//              dataType:'html',
//              success:function(data){
//                  $('.inner').prepend(data);
//              };
//          });*/
//        //BUT FOR EXAMPLE PURPOSES......
//        // We'll just simulate generation on server


//        //Simulate server delay;
//        setTimeout(function () {
//            // Simulate retrieving 4 messages
//            for (var i = 0; i < 4; i++) {
//                $('.msg_container_base').prepend('<div class="messages">Newly Loaded messages<br/><span class="date">' + Date() + '</span> </div>');
//            }
//            // Hide loader on success
//            //$('#loader').hide();
//            // Reset scroll
//            $('.msg_container_base').scrollTop(30);
//        }, 780);
//    }
//});


$(document).ready(function () {
    $.ajax({
        type: 'GET',
        url: '../../Notification/Chat/GetUnreadMessages',
        dataType: "json",
        success: function (resultData) { populateMessageSummary(resultData); },
        error: function (error) { alert(error); }
    });
});


