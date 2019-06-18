describe('Store Page Tests', function() {

  
        it('store has products - shows all stores products with buy button', function(){
            cy.visit('http://localhost:8080/wot/store/1')
            cy.get('.grid').find('#product_card')
        
            })
    
  })

  function addStore(){
    cy.visit('http://localhost:8080/wot/newstore')
    cy.get('#inputStorename').type('storeTest')
    cy.get('#doneaddstore_btn').click()

  }

  function login(){
    cy.visit('http://localhost:8080/wot/signin')    
    cy.get('#inputUserame').type('Admin')
    cy.get('#inputPassword').type('Admin')
    cy.get('#signin_btn').click()

  }
