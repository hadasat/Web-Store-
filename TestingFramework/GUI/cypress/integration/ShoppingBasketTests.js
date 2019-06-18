describe('Shopping basket test', function() {

    it('empty basket', function() {
      cy.visit('http://localhost:8080/wot/shoppingbasket')
      cy.get('.grid').find('#empty_title')
     
    })
  

  
    it('product exists', function() {
        addProductTobasket()
      cy.visit('http://localhost:8080/wot/shoppingbasket')
      cy.get('.grid').find('#cartContainer')
     
    })
  })


 function  addProductTobasket(){
    cy.visit('http://localhost:8080/wot/store/1/1');
    cy.get('#addToBasket').click()
  }