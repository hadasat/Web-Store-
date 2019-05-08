 // <!-- NavBar  Script -->
 function setUpOptBtn(){
    if(localStorage.getItem("loggedIn") === "true"){
        $("#nav_signin").hide();
        $("#nav_signOut").show();
        $("#nav_userName").append(`<span class="nav-link"><strong>Hello ${localStorage.getItem("username")}!</strong></span>`);
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
    // sendMessage({type: "action", info: "signout" , data: localStorage.getItem("userId")});
    signOut();
})

function signOut(){
    localStorage.setItem("loggedIn","false");
    localStorage.setItem("username","");
    setUpOptBtn();
}

setUpOptBtn();