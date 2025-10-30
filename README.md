Traveling Application

TravelingApplication is a comprehensive travel planning application that serves as a single interface connecting users to multiple travel-related services.
It provides weather information, currency exchange rates, country details, and booking options for hotels and flights. The application prioritizes user convenience by coordinating these services from a single entry point, offering a seamless travel planning experience.


---

Main Components

TravelingApplication
The main entry point of the project. It initializes and coordinates all other services, manages user requests, and handles authentication and authorization for secure access. All functionality—including weather, currency, hotel bookings, and flight reservations—is available through this application.

Authentication & Authorization
Users provide their name and email to obtain a JWT token. To access protected endpoints such as Hotel Booking and Flights Booking, users must include this token in the request headers. This ensures only authenticated users can perform bookings.

Weather Service
Provides real-time weather information and forecasts for selected travel destinations.

Currency Exchange Service
Displays up-to-date currency conversion rates to support international travel planning.

Hotel Booking Service
When the user enters a destination (city or country), check-in and check-out dates, and the number of adults and children, the application opens an external Booking.com page with the details prefilled, allowing the user to complete the booking there.

Flights Booking Service
When the user enters flight details such as origin, destination, and travel dates, the application opens an external Expedia page with the details prefilled, enabling the user to proceed with booking directly on Expedia.

Additional Information Service
Provides detailed country information, including capital city, Google Maps link, flag description and image, area, and population, giving users comprehensive knowledge about their travel destinations.



---

How It Works

1. Users log in through TravelingApplication and receive a JWT token for secure access.

2. The application uses this token to authenticate requests to hotel booking and flight booking services.

3. Users select the service they want and provide the required details. The TravelingApplication forwards the request to the corresponding service and delivers the results or opens the external page:

•Retrieves weather forecasts via the Weather Service

•Provides currency exchange rates via the Currency Exchange Service

•Opens Booking.com with prefilled hotel booking details via the Hotel Booking Service

•Opens Expedia with prefilled flight booking details via the Flights Booking Service

•Displays detailed country information via the Additional Information Service

4. All services are coordinated through TravelingApplication, ensuring a unified and user-friendly experience for travel planning.

