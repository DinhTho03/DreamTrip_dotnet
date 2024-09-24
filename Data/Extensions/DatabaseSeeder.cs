using brandportal_dotnet.Data.Entities;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using TravelItineraryProject.Configuration;
using TravelItineraryProject.Data.Entities;

namespace TravelItineraryProject.Data.Extensions;

public class DatabaseSeeder
{
    private readonly IMongoCollection<Account> _accountCollection;
    private readonly IMongoCollection<Addresses> _addressCollection;
    private readonly IMongoCollection<Categories> _cateCollection;
    private readonly IMongoCollection<Comments> _commentCollection;
    private readonly IMongoCollection<DayItinerary> _dayItineraryCollection;
    private readonly IMongoCollection<DetailTripPlan> _detailTripPlanCollection;
    private readonly IMongoCollection<ExcludedItems> _excludedItemsCollection;
    private readonly IMongoCollection<Favorites> _favoritesCollection;
    private readonly IMongoCollection<GoogleMapsAddress> _googleMapsAddressCollection;
    private readonly IMongoCollection<Images> _imagesCollection;
    private readonly IMongoCollection<IncludedItems> _includedItemsCollection;
    private readonly IMongoCollection<Ratings> _ratingsCollection;
    private readonly IMongoCollection<Service> _serviceCollection;
    private readonly IMongoCollection<ServicePlan> _servicePlanCollection;
    private readonly IMongoCollection<TripPlan> _tripPlanCollection;
    private readonly IMongoCollection<Role> _roleCollection;
    private readonly IMongoCollection<Faq> _faqCollection;
    private readonly IMongoCollection<FaqGroup> _faqGroupCollection;

    public DatabaseSeeder(IOptions<DatabaseSettings> options)
    {
        var client = new MongoClient(options.Value.ConnectionString);
        var database = client.GetDatabase(options.Value.DatabaseName);
        _roleCollection = database.GetCollection<Role>("role");
        _accountCollection = database.GetCollection<Account>("account");
        _addressCollection = database.GetCollection<Addresses>("addresses");
        _cateCollection = database.GetCollection<Categories>("categories");
        _commentCollection = database.GetCollection<Comments>("comments");
        _dayItineraryCollection = database.GetCollection<DayItinerary>("dayItinerary");
        _detailTripPlanCollection = database.GetCollection<DetailTripPlan>("detailTripPlan");
        _favoritesCollection = database.GetCollection<Favorites>("favorites");
        _googleMapsAddressCollection = database.GetCollection<GoogleMapsAddress>("googleMapsAddress");
        _imagesCollection = database.GetCollection<Images>("images");
        _includedItemsCollection = database.GetCollection<IncludedItems>("includedItems");
        _excludedItemsCollection = database.GetCollection<ExcludedItems>("excludedItems");
        _ratingsCollection = database.GetCollection<Ratings>("ratings");
        _serviceCollection = database.GetCollection<Service>("service");
        _servicePlanCollection = database.GetCollection<ServicePlan>("servicePlan");
        _tripPlanCollection = database.GetCollection<TripPlan>("tripPlan");
        _faqCollection = database.GetCollection<Faq>("faq");
        _faqGroupCollection = database.GetCollection<FaqGroup>("faqGroup");
    }

    public void Seed()
    {
        // Create data for Role
        var RoleList = new List<Role>
        {
            new Role
            {
                _Id = "64eaf9050367766aeffb8eb4",
                name = "admin"
            },
            new Role
            {
                _Id = "64eaf9050367766aeffb8eb5",
                name = "user"
            }
        };
        if (_roleCollection.CountDocuments(_ => true) == 0)
        {
            _roleCollection.InsertMany(RoleList);
        }

        // Create data for Account
        var userList = new List<Account>
        {
            new Account
            {
                _Id = "655f9dca62b255ca9b9dcab5",
                firstName = "John",
                lastName = "Doe",
                email = "john.doe@example.com",
                phone = "123-456-7890",
                password = "$2b$12$VJQLw/P4FzCZ7.3IKj5xZ.AdnEk1wZMZWNiBeQQ1Xe0PQZ29GL7w2",
                passwordRT = "",
                roleId = "64eaf9050367766aeffb8eb4", // Replace with an actual role ID
                registerDate = DateTime.Now
            },
            new Account
            {
                _Id = "656499cccf3a97431ceabc92",
                firstName = "Jane",
                lastName = "Smith",
                email = "jane.smith@example.com",
                phone = "987-654-3210",
                password = "$2b$12$hGXU3kJC4Bx6yl6HZjG7aOS7fsy/vp02LhV.uSaH55ovEk2UHzIuq",
                passwordRT = "",
                roleId = "64eaf9050367766aeffb8eb4", // Replace with another actual role ID
                registerDate = DateTime.Now.AddDays(-1)
            }
        };
        if (_accountCollection.CountDocuments(_ => true) == 0)
        {
            _accountCollection.InsertMany(userList);
        }

        // 


        // Create data for Categories
        var categoryList = new List<Categories>
        {
            new Categories
            {
                _Id = "656170e3c44dbd18639e0624",
                name = "Adventure Trip"
            },
            new Categories
            {
                _Id = "65645f2f2884711f5decfe27",
                name = "Family Trip"
            },
            new Categories
            {
                _Id = "656466142884711f5ded0688",
                name = "Heritage Trip"
            },
        };

        if (_cateCollection.CountDocuments(_ => true) == 0)
        {
            _cateCollection.InsertMany(categoryList);
        }



        // Create data for Service
        var serviceList = new List<Service>
        {
            new Service
            {
                _Id = "6564689c2884711f5ded091c",
                name = "Romantic Maldives",
                description = "t is a long established fact that a reader will be distracted by the readable content of a page when looking at its layout. The point of using Lorem Ipsum is that it has a more-or-less normal distribution of letters, as opposed to using 'Content here, content here', making it look like readable English. Many desktop publishing packages and web page editors now use Lorem Ipsum as their default model text, and a search for 'lorem ipsum' will uncover many web sites still in their infancy. Various versions have evolved over the years, sometimes by accident, sometimes on purpose (injected humour and the like).",
                price = 9.99m,
                created = DateTime.Now,
                updated = null,
                qualityView = 1080,
                cateId = "65645f2f2884711f5decfe27" // Replace with an actual category ID
            },
            new Service
            {
                _Id = "65646aef2884711f5ded092a",
                name = "Basic Subscription",
                description = "Access to basic content",
                price = 4.99m,
                created = DateTime.Now.AddDays(-1),
                updated = DateTime.Now.AddHours(-1),
                qualityView = 720,
                cateId = "65645f2f2884711f5decfe27"// Replace with another actual category ID
            }
        };

        if (_serviceCollection.CountDocuments(_ => true) == 0)
        {
            _serviceCollection.InsertMany(serviceList);
        }


        // Create data for Addresses
        var addressList = new List<Addresses>
        {
            new Addresses
            {
                _Id = "656471952884711f5ded0b34",
                country = "Maldives",
                province = "California",
                district = "Los Angeles",
                destailStreet = "123 Main Street",
                servicePlanId = "6564689c2884711f5ded091c"// Replace with an actual service plan ID
            },
            new Addresses
            {
                _Id = "65159c8bbf54a316e45e751d",
                country = "Canada",
                province = "Ontario",
                district = "Toronto",
                destailStreet = "456 Maple Avenue",
                servicePlanId = "65646aef2884711f5ded092a"// Replace with another actual service plan ID
            },
        };

        if (_addressCollection.CountDocuments(_ => true) == 0)
        {
            _addressCollection.InsertMany(addressList);
        }

        // Create data for GoogleMapsAddress 
        var googleMapsAddressList = new List<GoogleMapsAddress>
        {
            new GoogleMapsAddress
            {
                _Id =  "6568aa6d156925755d7bd380",
                location = "40.7128,-74.0060", // Example: Latitude, Longitude
                fotmattedAddress = "123 Main St, City, Country",
                addressId = "656471952884711f5ded0b34"// Replace with an actual address ID
            },
            new GoogleMapsAddress
            {
                _Id = "6568aac5156925755d7bd38b",
                location = "34.0522,-118.2437", // Example: Latitude, Longitude
                fotmattedAddress = "456 Maple Ave, Another City, Another Country",
                addressId =  "65646aef2884711f5ded092a" // Replace with another actual address ID
            },
        };

        if (_googleMapsAddressCollection.CountDocuments(_ => true) == 0)
        {
            _googleMapsAddressCollection.InsertMany(googleMapsAddressList);
        }

        // Create data for Images
        var imagesList = new List<Images>
        {
            new Images
            {
                _Id = "6568ab19156925755d7bd394",
                url = "https://lh3.google.com/u/0/d/1-q65NNQgo2utInZESbmnRaOESJM8DMeC=w2560-h1271-iv2", // Replace with an actual image URL
                servicePlanId = "6564689c2884711f5ded091c"// Replace with an actual service plan ID
            },
            new Images
            {
                _Id = "6568ab71156925755d7bd74f",
                url = "https://example.com/image2.jpg", // Replace with another actual image URL
                servicePlanId = "65646aef2884711f5ded092a" // Replace with another actual service plan ID
            },
        };

        if (_imagesCollection.CountDocuments(_ => true) == 0)
        {
            _imagesCollection.InsertMany(imagesList);
        }

        // Create data for ExcludedItems
        var excludedItemsList = new List<ExcludedItems>
        {
            new ExcludedItems
            {
                _Id =  "65645a7e461c0cc9648e9e10",
                title = "Sight-seen",
                servicePlanId = "65646aef2884711f5ded092a" // Replace with an actual service plan ID
            },
            new ExcludedItems
            {
                _Id = "6565503fda4387eeaa45622d",
                title = "City Tour",
                servicePlanId = "6564689c2884711f5ded091c" // Replace with another actual service plan ID
            },
            new ExcludedItems
            {
                _Id = "6565503fda4387eeaa45622e",
                title = "Sight-seen",
                servicePlanId = "6564689c2884711f5ded091c" // Replace with another actual service plan ID
            },
        };

        if (_excludedItemsCollection.CountDocuments(_ => true) == 0)
        {
            _excludedItemsCollection.InsertMany(excludedItemsList);
        }

        // Create data for IncludedItems
        var includedItemsList = new List<IncludedItems>
        {
            new IncludedItems
            {
                _Id = "6565504bda4387eeaa456234",
                title = "Flight Ticket & Cab Transportation",
                servicePlanId = "6564689c2884711f5ded091c" // Replace with an actual service plan ID
            },
            new IncludedItems
            {
                _Id = "6565504bda4387eeaa456235",
                title = "Breakfast, Lunch & Dinner",
                servicePlanId = "6564689c2884711f5ded091c" // Replace with an actual service plan ID
            },
            new IncludedItems
            {
                _Id = "6565504bda4387eeaa456236",
                title = "Hotel Accommodation",
                servicePlanId = "6564689c2884711f5ded091c" // Replace with an actual service plan ID
            },
            new IncludedItems
            {
                _Id = "6565504bda4387eeaa456237",
                title = "Professional Tour Guide",
                servicePlanId = "6564689c2884711f5ded091c" // Replace with an actual service plan ID
            },
            new IncludedItems
            {
                _Id = "65658ad8e1613c2bfcef7f1b",
                title = "Premium Features",
                servicePlanId = "65646aef2884711f5ded092a"// Replace with another actual service plan ID
            },
        };

        if (_includedItemsCollection.CountDocuments(_ => true) == 0)
        {
            _includedItemsCollection.InsertMany(includedItemsList);
        }


        // Create data for ServicePlan
        var servicePlanList = new List<ServicePlan>
        {
            new ServicePlan
            {
                _Id = "6567647272426e7151749bb8",
                title = "Basic Plan",
                description = "Access to basic features",
                url = "https://example.com/basic-plan",
                startDate = DateTime.Now,
                endDate = DateTime.Now.AddMonths(6), // Example: 6 months validity
                serviceId = "65646aef2884711f5ded092a" // Replace with an actual service plan ID
            },
            new ServicePlan
            {
                _Id = "656864cb1b3b041a536969f9",
                title = "Buổi sáng",
                description = "Content here, content here', making it look like readable English. Many desktop publishing packages and web page editors now use Lorem Ipsum as their default model text, and a search for 'lorem ipsum' will uncover many web sites still in their infancy. ",
                url = "https://example.com/premium-plan",
                startDate = DateTime.Now,
                endDate = DateTime.Now.AddYears(1), // Example: 1 year validity
                serviceId = "6564689c2884711f5ded091c" // Replace with another actual service plan ID
            },
        };

        if (_servicePlanCollection.CountDocuments(_ => true) == 0)
        {
            _servicePlanCollection.InsertMany(servicePlanList);
        }

        // Create data for Ratings
        var ratingList = new List<Ratings>
        {
            new Ratings
            {
                _Id = "6568a260156925755d7b994f",
                userId = "656499cccf3a97431ceabc92", // Replace with an actual user ID
                servicePlanId = "65646aef2884711f5ded092a", // Replace with an actual service plan ID
                value = 4.5 // Replace with an actual rating value
            },
            new Ratings
            {
                _Id = "6568a27b156925755d7b9bb7",
                userId =  "655f9dca62b255ca9b9dcab5", // Replace with another actual user ID
                servicePlanId = "6564689c2884711f5ded091c", // Replace with another actual service plan ID
                value = 5 // Replace with another actual rating value
            },
            // Add more ratings as needed
        };

        // Assuming _ratingCollection is your MongoDB Rating collection
        if (_ratingsCollection.CountDocuments(_ => true) == 0)
        {
            _ratingsCollection.InsertMany(ratingList);
        }

        // Create data for Favorites
        var favoritesList = new List<Favorites>
        {
            new Favorites
            {
                _Id = "6568b6480401a5aef22d59a6",
                userId =  "655f9dca62b255ca9b9dcab5", // Replace with an actual user ID
                itemId = "6564689c2884711f5ded091c", // Replace with an actual item ID
                itemType = "Service" // Replace with the actual item type (e.g., "Video")
            },
            new Favorites
            {
                _Id = "6568b6880401a5aef22d59ad",
                userId = "656499cccf3a97431ceabc92", // Replace with another actual user ID
                itemId = "6564689c2884711f5ded091c", // Replace with another actual item ID
                itemType = "Service" // Replace with the actual item type (e.g., "Service")
            },
        };

        if (_favoritesCollection.CountDocuments(_ => true) == 0)
        {
            _favoritesCollection.InsertMany(favoritesList);
        }

        // Create data for Comments            
        var commentsList = new List<Comments>
        {
            new Comments
            {
                _Id = "65694cf678d9c69deafd64a7",
                text = "Great service!",
                url = "https://example.com/comment1",
                createDate = DateTime.Now,
                updateDate = DateTime.Now,
                userId = "656499cccf3a97431ceabc92", // Replace with an actual user ID
                servicePlanId = "6564689c2884711f5ded091c" // Replace with an actual service plan ID
            },
            new Comments
            {
                _Id = "655854f4f57c2589759a1185",
                text = "Excellent experience!",
                url = "https://example.com/comment2",
                createDate = DateTime.Now,
                updateDate = DateTime.Now,
                userId = "655f9dca62b255ca9b9dcab5", // Replace with another actual user ID
                servicePlanId = "65646aef2884711f5ded092a" // Replace with another actual service plan ID
            },
        };

        if (_commentCollection.CountDocuments(_ => true) == 0)
        {
            _commentCollection.InsertMany(commentsList);
        }

        // Create data for DayItinerary
        var dayItineraryList = new List<DayItinerary>
            {
                new DayItinerary
                {
                    _Id = "656949800fd3d398f9580a8c",
                    dayNumber = 1,
                    activities = "Maldives",
                    userId = "655f9dca62b255ca9b9dcab5" // Replace with an actual user ID
                },
                new DayItinerary
                {
                    _Id = "65694cf578d9c69deafd64a0",
                    dayNumber = 1,
                    activities = "Visit museums and art galleries",
                    userId = "656499cccf3a97431ceabc92" // Replace with another actual user ID
                },
            };

        if (_dayItineraryCollection.CountDocuments(_ => true) == 0)
        {
            _dayItineraryCollection.InsertMany(dayItineraryList);
        }



        // Create data for TripPlan    
        var tripPlanList = new List<TripPlan>
            {
                new TripPlan
                {
                    _Id = "6561706954b4398e48af3a9b",
                    title = "Maldives",
                    description = "A thrilling mountain exploration",
                    startDate = DateTime.Now,
                    endDate = DateTime.Now.AddDays(7),
                    dayNumber = 1,
                    dayItineraryId = "656949800fd3d398f9580a8c"
                },
                new TripPlan
                {
                    _Id = "65645f2f2884711f5decfe29",
                    title = "Beach Vacation",
                    description = "Relaxing time by the beach",
                    startDate = DateTime.Now.AddDays(8),
                    endDate = DateTime.Now.AddDays(14),
                    dayNumber = 1,
                    dayItineraryId = "65694cf578d9c69deafd64a0"
                },
            };

        if (_tripPlanCollection.CountDocuments(_ => true) == 0)
        {
            _tripPlanCollection.InsertMany(tripPlanList);
        }




        // Create data for DetailTripPlan    
        var detailTripPlanList = new List<DetailTripPlan>
            {
                new DetailTripPlan
                {
                    _Id = "656460b82884711f5decfeae",
                    location = "City A",
                    nameService = "Hotel XYZ",
                    description = "Comfortable hotel with a view",
                    price = "$150 per night",
                    startDate = DateTime.Now,
                    endDate = DateTime.Now.AddDays(1), // Example: 3 days stay
                    tripPlanId = "6561706954b4398e48af3a9b" // Replace with an actual trip plan ID
                },
                new DetailTripPlan
                {
                    _Id = "656460b82884711f5decfeb1",
                    location = "City B",
                    nameService = "Tour Company ABC",
                    description = "Guided tour of local attractions",
                    price = "$200 per person",
                    startDate = DateTime.Now, // Example: Start after the first service
                    endDate = DateTime.Now.AddDays(1), // Example: 3 days tour
                    tripPlanId = "65645f2f2884711f5decfe29" // Replace with another actual trip plan ID
                },
            };

        if (_detailTripPlanCollection.CountDocuments(_ => true) == 0)
        {
            _detailTripPlanCollection.InsertMany(detailTripPlanList);
        }


        var faqGroupList = new List<FaqGroup>
{
    new FaqGroup
    {
        ParentId = null, // Example of a root FAQ group
        FaqGroupType = "basic",
        FaqGroupTitle = "General Information",
        FaqGroupPosition = 1,
        IsActived = true,
        IsDeleted = false,
        CreatedAt = DateTime.Now,
        CreatedBy = 123, // Example user ID
        UpdatedAt = DateTime.Now,
        UpdatedBy = 123
    },
    new FaqGroup
    {
        ParentId = null, // Example of a sub-FAQ group
        FaqGroupType = "basic",
        FaqGroupTitle = "Payment Information",
        FaqGroupPosition = 2,
        IsActived = true,
        IsDeleted = false,
        CreatedAt = DateTime.Now,
        CreatedBy = 124, // Example user ID
        UpdatedAt = DateTime.Now,
        UpdatedBy = 124
    }
};

        if (_faqGroupCollection.CountDocuments(_ => true) == 0)
        {
            _faqGroupCollection.InsertMany(faqGroupList);
        }

        var faqList = new List<Faq>
{
    new Faq
    {
        _Id = "636460b82884711f5decfea1", // Generate new ObjectId
        FaqGroup = "646460b82884711f5decfeae", // Example group ID (should match an existing group)
        FaqType = "basic", // Type can be 'faq', 'policy', 'terms', etc.
        FaqTitle = "How to register?",
        FaqContent = "You can register by clicking the 'Register' button at the top right of the homepage.",
        FaqPosition = 1, // Example display order
        IsActived = true,
        IsDeleted = false,
        CreatedAt = DateTime.Now,
        CreatedBy = 123, // Example creator user ID
        UpdatedAt = DateTime.Now,
        UpdatedBy = 123
    },
    new Faq
    {
        _Id = "646460b82884711f5decfea2", // Generate another ObjectId
        FaqGroup = "646460b82884711f5decfea3", // Same group or different, depending on structure
        FaqType = "basic", // This is a policy type FAQ
        FaqTitle = "Privacy Policy",
        FaqContent = "Your privacy is important to us. We do not share your information with third parties.",
        FaqPosition = 2,
        IsActived = true,
        IsDeleted = false,
        CreatedAt = DateTime.Now,
        CreatedBy = 124, // Another example creator user ID
        UpdatedAt = DateTime.Now,
        UpdatedBy = 124
    }
};

        if (_faqCollection.CountDocuments(_ => true) == 0)
        {
            _faqCollection.InsertMany(faqList);
        }



    }




}