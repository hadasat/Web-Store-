describe('The Home Page', function() {
  /*it('successfully loads', function() {
    cy.visit('http://localhost:8080/wot')
    
   
  })*/

  it('header is exist', function() {
    cy.visit('http://localhost:8080/wot')
    cy.get('h1').should('contain', 'Stores')
   
  })
})