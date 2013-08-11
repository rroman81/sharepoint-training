// Place custom JavaScript below
var ctx;
var web;
var user;

function sharePointReady() {
    this.ctx = new SP.ClientContext.get_current();

    $("#getListCount").click(function (event) {
        getWebProperties();
        event.preventDefault();
    });
    welcome();
}


function welcome() {
    this.web = ctx.get_web();
    this.user = web.get_currentUser();
    this.ctx.load(user);

    this.ctx.executeQueryAsync(onUserReqSuccess, onUserReqFail);
}

function onUserReqSuccess() {
    var welcomeText = document.getElementById("starter");
    var userWelcome = document.createElement("p");
    userWelcome.style.fontSize = "14pt";
    userWelcome.innerText = "Welcome " + user.get_loginName() + ".";
    welcomeText.appendChild(userWelcome);
}

function onUserReqFail(sender, args) {
    alert('Failed to find current user. ' + args.get_message());
}


function getWebProperties() {
    this.web = ctx.get_web();
    this.lists = this.web.get_lists();
    this.ctx.load(this.lists);
    this.ctx.executeQueryAsync(Function.createDelegate(this, this.onSuccess), Function.createDelegate(this, this.onFail));
}

function onSuccess(sender, args) {
    alert('Number of lists in web:' + this.lists.get_count());
}

function onFail(sender, args) {
    alert('failed to get list. Error:' + args.get_message());
}

