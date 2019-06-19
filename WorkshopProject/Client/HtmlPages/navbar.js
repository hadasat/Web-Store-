//Nav Bar Init Html

document.getElementById("navbar_header").innerHTML = `
<nav class="navbar navbar-expand-lg navbar-light bg-light">
        <a class="navbar-brand" href="/wot/main">WorkShop</a>
        <button class="navbar-toggler" type="button" data-toggle="collapse" data-target="#navbarSupportedContent" aria-controls="navbarSupportedContent" aria-expanded="false" aria-label="Toggle navigation">
            <span class="navbar-toggler-icon"></span>
        </button>

        <div class="collapse navbar-collapse" id="navbarSupportedContent">
            <ul class="navbar-nav mr-auto">
                <li id="nav_signin" class="nav-item">
                    <a class="nav-link" href="/wot/signin">Sign In</a>
                </li>

                <!-- visible only when the user is logged in -->
                <li id="nav_userName" class="nav-item"></li>
                <li id="nav_signOut" class="nav-item">
                    <a class="nav-link" href="#">Sign Out</a>
                </li>
                <li id="nav_opensStore" class="nav-item">
                    <a class="nav-link" href="/wot/newstore">New Store </a>
                </li>

                <!-- end  -->

                <li id="nav_shopping_basket" class="nav-item">
                    <a class="nav-link" href="/wot/shoppingbasket">Shopping Basket</a>
                </li>

            </ul>
            <ul class="navbar-nav ml-auto">
                <li id="nav_admin" class="nav-item">
                    <a class="nav-link" href="/wot/adminactions">Admin Actions</a>
                </li>
                <li id="nav_advanced_search" class="nav-item">
                    <a class="nav-link" href="/wot/search">Advance Search </a>
                </li>
            </ul>
        </div>
    </nav>
`;

// <!-- NavBar  Script -->
 function setUpOptBtn(){
     
     const userStatus = sessionStorage.getItem("user_status");
     const loggedIn = userStatus? JSON.parse(userStatus).loggedIn : false;
     const username = userStatus? JSON.parse(userStatus).username : '';
    if(loggedIn){
        $("#nav_signin").hide();
        $("#nav_signOut").show();
        $("#nav_userName").append(`<span class="nav-link"><strong>Hello ${username}!</strong></span>`);
        $("#nav_userName").show();
        $("#nav_opensStore").show();
        
    }else{

        $("#nav_signin").show();
        $("#nav_signOut").hide();
        $("#nav_userName").hide();
        $("#nav_opensStore").hide();
    }

}

$('#nav_signOut').on('click', ()=>{
    signOut();
})

function signOut(){
    sendRequest("action","signOut",{}).then(function(msg){
        sessionStorage.setItem("user_status",JSON.stringify({loggedIn : false, username: ''}));
        setUpOptBtn();
        window.location.href = "/wot/main";
    })
}


function updateAdmin(){
    
    sendRequest('data','isAdmin',{}).then(function(isAdmin){
        if(isAdmin)
        $("#nav_admin").show();
    else
        $("#nav_admin").hide();

    });


}

setUpOptBtn();