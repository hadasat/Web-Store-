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
                    <a class="nav-link" href="#">New Store </a>
                </li>

                <!-- end  -->

                <li class="nav-item">
                    <a class="nav-link" href="#">Shopping Cart</a>
                </li>

            </ul>
            <ul class="navbar-nav ml-auto">
                <li class="nav-item">
                    <a class="nav-link" href="#">Advance Search</a>
                </li>
            </ul>

            <form class="form-inline my-2 my-lg-0">
                <input class="form-control mr-sm-2" type="search" placeholder="Search" aria-label="Search">
                <button class="btn btn-outline-success my-2 my-sm-0" type="submit">Search</button>
            </form>
        </div>
    </nav>
`;


// <!-- NavBar  Script -->
 function setUpOptBtn(){
     const userStatus = localStorage.getItem("user_status");
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
        localStorage.setItem("user_status",JSON.stringify({loggedIn : false, username: ''}));
        setUpOptBtn();
    })
}

setUpOptBtn();