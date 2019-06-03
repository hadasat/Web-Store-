describe('The Home Page', function() {
  /*it('successfully loads', function() {
    cy.visit('http://localhost:8080/wot')
    
   
  })*/

  it('header is exist', function() {
    cy.visit('http://localhost:8080/wot')
    cy.get('h1').should('contain', 'Stores')
   
  })

  it('nav bar exists', function() {
    cy.visit('http://localhost:8080/wot')
    cy.get('#navbar_header').should('be.visible')
	// todo copy to all webpages and kill ofir
  })
  
  it('nav bar for guests', function() {
    cy.visit('http://localhost:8080/wot')
    cy.get('#navbar_header').get('#nav_signin').should('be.visible')
	cy.get('#navbar_header').get('#nav_advanced_search').should('be.visible')
	cy.get('#navbar_header').get('#nav_shopping_basket').should('be.visible')
	cy.get('.navbar-brand').should('be.visible')
	cy.get('#navbar_header').get('#nav_userName').should('not.be.visible')
	cy.get('#navbar_header').get('#nav_signOut').should('not.be.visible')
	cy.get('#navbar_header').get('#nav_opensStore').should('not.be.visible')
	cy.get('#navbar_header').get('#nav_admin').should('not.be.visible')
	// todo copy to all webpages and kill ofir
  })
  
  
})