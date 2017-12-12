const artyom = new Artyom();

var UserDictation = artyom.newDictation({
    continuous: true, // Enable continuous if HTTPS connection
    onResult: function (text) {
        
        // Do something with the text
        if(text.indexOf('go to')>0 || text.indexOf('goto')>0)
        {
            if(text.indexOf('student')>0)
            {
                window.location.href = '~/Student/Student/Search';
            }
            else if(text.indexOf('staff')>0)
            {
                window.location.href = '~/User/User/Search';
            }
        }
    },
    onStart: function () {
        console.log("Dictation started by the user");
    },
    onEnd: function () {
        alert("Dictation stopped by the user");
    },
    onError: function (e) {
        
        console.log(e.error);
    }
});

UserDictation.start();