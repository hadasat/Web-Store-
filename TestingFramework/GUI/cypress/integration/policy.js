describe('Policy Gui Test', function() {

  
    it('policy header exsit', function(){
        cy.visit('http://localhost:8080/wot/policies/1')
        cy.get('body').find('#add_policy')
    
        })

})