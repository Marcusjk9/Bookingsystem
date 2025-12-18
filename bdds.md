 ### BDD Scenarios @ Wireframes ###


Excalidraw 1. link: https://excalidraw.com/#room=da6cb73c6d10a098489a,npNcQ6K5GchMZO_71JoLWg
Excalidraw 2. Llink https://excalidraw.com/#room=44e936580ef6298f723c,K9hAhUIR-RVu3mVWLMaHxg


# As a visitor:

Given I am a visitor on the homepage
When I click "Search hotels"
And I type "Stockholm" in the city box
And I press "Search"
Then I see a result list of hotels
And I can click one hotel to see more info

Given I am a visitor (not loged in)
When I go to the Admin page
Then I should be send back to login page
And I see a message like "you need login first"

Given I am on the main page 
and I have made a search with no info 
when i press the search button 
Then I want info 
that I have to enter search criteria

# As a user:

Given I am on login page
When I write my email "user@example.com"
And I write my password
And I click "Login"
Then I see home page
And I see "Welcome" message
to my profile page

Given I am on login page
When I write my email
And I write wrong password
And I click "Login"
Then I see error "Wrong password"
And I stay on login page

Given I am logged in as user  
When I search hotels
And I want to filter pets allowed
And I click cross pet allowed
Then I see the results with pet allowed
And I can book those hotels

Given i want to made one booking
And i see listed offer
when i choose one offer
And i comming in in details page
Then i want to choose how many person i booking for
And i can clearly see what will be price

Given im found one offer
And i want to book the hotel
when i choose it
And i comming in in details page
Then i want to choose how many rooms i reserv
And i can clearly see what is the pric difference between one room and many rooms

Given im logged in as user
And i has previous bookings and travel history
When i open "Recommended for you"
Then i see travel suggestions based on previous bookings and interests

Given im logged in as user
And i has a booking where end date is passed
When i give rating 1-5 and write comment
And i press "Submit rating"
Then rating is saved and shown for other guests

Given im logged in as user
And im is on booking details page
When i write message to accommodation or activity responsible
And i press "Send"
Then message is delivered to the travel agancy

Given I see hotels in "Halmstad"
When I pick filter "Distance to City Center"
And I pick "Less than 4 km"
Then I see hotels near this circle
And hotels sort by distance

Given I am on the main page and 
I have made a previous search 
and updated the search critiera when i press the search button 
Then I want new travels options based on new criteria

# As a employee:

Given I am on the website login page for staff
When I type employee email and password
And I press "Login"
Then I should be logged in as employee
And I should see employee dashboard page

Given I am logged in as employee
And I am on "Bookings" page in admin/employee area
When I enter search criteria for booking (like customer email or booking id)
And I press "Search"
Then I should see list of bookings matching my search criteria
And I can open one booking details to help customer

Given I am logged in as employee
And I am on "Support tickets" page
When I click on one support ticket in the list
Then I should see ticket details (message, user contact, booking reference)
And I can continue helping next costumer with this info

Given that Im on any page 
and I've clicked log in 
and I've entered correct login 
when I press login 
I want to get logged in 
and see my name in the up right corner

# As a admin:

Given I am admin
And I see booking #12345
When I click "Cancel Booking"
And I say yes
Then booking is cancelled
And customer gets email
And rooms are free again

Given I login as admin
When I click "Add Hotel"
And I write hotel name "City Hotel"
And I write location
And I write rooms "20"
And I click "Save"
Then hotel is in system
And users can find it

Given I am on the Admin login page
When I type my email and password
And I click "Login"
Then I should land on the admin dashbord

Given I am on the Admin login page
When I enter wrong password
And I click "Login"
Then I stay on same page
And I see error "Wrong email or passwrod"

Given im logged in as a admin
When i review hotels fedback
And one of the hotels have bad feedbacks
Then i want to be able to delete or suspend this hotel
And i see that this hotel is not in live offers anymore