describe('Log in Process', function() {

    it('Verify alert when login failed', function(){
        cy.visit('http://localhost:8080/wot/signin')    
    
        const stub = cy.stub()  
        cy.on ('window:alert', stub)
        cy
        .get('#signin_btn').click()
        .then(() => {
          expect(stub.getCall(0)).to.be.calledWith('Signin error: Please fill all the required fields')      
        })  
    
        })

        it('login succeed - user is redirected to main page', function(){
            cy.visit('http://localhost:8080/wot/signin')    
            cy.get('#inputUserame').type('Admin')
            cy.get('#inputPassword').type('Admin')
            cy.get('#signin_btn').click()
            cy.location('pathname', {timeout: 1000}).should('eq', '/wot/main')
        
            })
    
  })

  describe('Register Process', function() {

    it('Verify alert when register failed', function(){
        cy.visit('http://localhost:8080/wot/signin')    
    
        const stub = cy.stub()  
        cy.on ('window:alert', stub)
        cy
        .get('#register_btn').click()
        .then(() => {
          expect(stub.getCall(0)).to.be.calledWith('Registration error: Please fill all the required fields')      
        })  
    
        })

        it('Register succeed - user is redirected to main page', function(){
            cy.visit('http://localhost:8080/wot/signin')    
            cy.get('#inputUserame').type('1')
            cy.get('#inputPassword').type('1')
            cy.get('#inputDate').type('12-12-1234')
            cy.get('#inputCountry').type('1')
            cy.get('#register_btn').click()
            cy.location('pathname', {timeout: 1000}).should('eq', '/wot/main')
        
            })
    
  })