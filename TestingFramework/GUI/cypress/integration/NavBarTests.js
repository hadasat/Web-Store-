describe('The nav bar test', function() {
  
  it('nav bar for guests', function() {
    cy.visit('http://localhost:8080/wot')
	nav_bar_guest_test()
	cy.visit('http://localhost:8080/wot/signin')
	nav_bar_guest_test()
	cy.visit('http://localhost:8080/wot/shoppingbasket')
	nav_bar_guest_test()
	cy.visit('http://localhost:8080/wot/search')
	nav_bar_guest_test()
	cy.visit('http://localhost:8080/wot/store/0')
	nav_bar_guest_test()
	//todo add for product page and fill ofir
  })

it('nav bar for admin', function() {
    cy.visit('http://localhost:8080/wot/signin')
	cy.get('#inputUserame').type('Admin')
	cy.get('#inputPassword').type('Admin')
	cy.get('#signin_btn').click ()
	nav_bar_admin_test()
	cy.visit('http://localhost:8080/wot/signin')
	nav_bar_admin_test()
	cy.visit('http://localhost:8080/wot/shoppingbasket')
	nav_bar_admin_test()
	cy.visit('http://localhost:8080/wot/search')
	nav_bar_admin_test()
	cy.visit('http://localhost:8080/wot/store/0')
	nav_bar_admin_test()
	//todo add for product page and fill ofir
	//todo add for logged in and admin
  })
  
  it('nav bar for member', function() {
    cy.get('#nav_signOut > .nav-link').click()
	cy.visit('http://localhost:8080/wot/signin')
	cy.get('#inputUserame').type('username')
	cy.get('#inputPassword').type('password')
	cy.get('#signin_btn').click ()
	nav_bar_member_test()
	cy.visit('http://localhost:8080/wot/signin')
	nav_bar_member_test()
	cy.visit('http://localhost:8080/wot/shoppingbasket')
	nav_bar_member_test()
	cy.visit('http://localhost:8080/wot/search')
	nav_bar_member_test()
	cy.visit('http://localhost:8080/wot/store/0')
	nav_bar_member_test()
	//todo add for product page and fill ofir
	//todo add for logged in and admin
  })
  
  
})


function nav_bar_guest_test (){
	cy.get('#navbar_header').should('be.visible')
	cy.get('.navbar-brand').should('be.visible')
    cy.get('#navbar_header').get('#nav_signin').should('be.visible')
	cy.get('#navbar_header').get('#nav_advanced_search').should('be.visible')
	cy.get('#navbar_header').get('#nav_shopping_basket').should('be.visible')
	cy.get('#navbar_header').get('#nav_userName').should('not.be.visible')
	cy.get('#navbar_header').get('#nav_signOut').should('not.be.visible')
	cy.get('#navbar_header').get('#nav_opensStore').should('not.be.visible')
	cy.get('#navbar_header').get('#nav_admin').should('not.be.visible')	
}

function nav_bar_member_test (){
	cy.get('#navbar_header').should('be.visible')
	cy.get('.navbar-brand').should('be.visible')
    cy.get('#navbar_header').get('#nav_signin').should('not.be.visible')
	cy.get('#navbar_header').get('#nav_advanced_search').should('be.visible')
	cy.get('#navbar_header').get('#nav_shopping_basket').should('be.visible')
	cy.get('#navbar_header').get('#nav_userName').should('be.visible')
	cy.get('#navbar_header').get('#nav_signOut').should('be.visible')
	cy.get('#navbar_header').get('#nav_opensStore').should('be.visible')
	cy.get('#navbar_header').get('#nav_admin').should('not.be.visible')	
}

function nav_bar_admin_test (){
	cy.get('#navbar_header').should('be.visible')
	cy.get('.navbar-brand').should('be.visible')
    cy.get('#navbar_header').get('#nav_signin').should('not.be.visible')
	cy.get('#navbar_header').get('#nav_advanced_search').should('be.visible')
	cy.get('#navbar_header').get('#nav_shopping_basket').should('be.visible')
	cy.get('#navbar_header').get('#nav_userName').should('be.visible')
	cy.get('#navbar_header').get('#nav_signOut').should('be.visible')
	cy.get('#navbar_header').get('#nav_opensStore').should('be.visible')
	cy.get('#navbar_header').get('#nav_admin').should('be.visible')	
}