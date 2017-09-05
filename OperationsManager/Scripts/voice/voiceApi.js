$(document).ready(function () {
    if (annyang) {
        // Let's define our first command. First the text we expect, and then the function it should call
        //var commands = {
        //    'show tps report': function () {
        //        $('#tpsreport').animate({ bottom: '-100px' });
        //    }
        //};

        var commands = {
            // annyang will capture anything after a splat (*) and pass it to the function.
            // e.g. saying "Show me Batman and Robin" is the same as calling showFlickr('Batman and Robin');
            'google it *tag': googleByTag,

            // A named variable is a one word variable, that can fit anywhere in your command.
            // e.g. saying "calculate October stats" will call calculateStats('October');
            'goto :pageName page': redirectTo,

            // By defining a part of the following command as optional, annyang will respond to both:
            // "say hello to my little friend" as well as "say hello friend"
            '(say )hello (to my little)( friend)': greeting,

            'goto :pageName': redirectTo
        };

        // Add our commands to annyang
        annyang.addCommands(commands);

        // Start listening. You can call this here, or attach this call to an event, button, etc.
        annyang.start();
    }

    var googleByTag = function (tag) {
        alert("Can't access google now!!!");
    }

    var redirectTo = function (pageName) {
        if(pageName=='search student'||pageName=='student search')
        {
            window.location.href = "~/Student/Student/Search";
        }
        if(pageName=='search staff'||pageName=='staff search')
        {
            window.location.href = "~/User/User/Search";
        }
    }

    var greeting = function () {
        responsiveVoice.speak("Hello!!!");
    }
});