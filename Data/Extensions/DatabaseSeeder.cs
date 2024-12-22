using brandportal_dotnet.Configuration;
using brandportal_dotnet.Data.Entities;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace brandportal_dotnet.Data.Extensions;

public class DatabaseSeeder
{
    private readonly IMongoCollection<Account> _accountCollection;
    private readonly IMongoCollection<Addresses> _addressCollection;
    private readonly IMongoCollection<Categories> _cateCollection;
    private readonly IMongoCollection<Comments> _commentCollection;
    private readonly IMongoCollection<GroupTripPlan> _dayItineraryCollection;
    private readonly IMongoCollection<DetailTripPlan> _groupTripPlanCollection;
    private readonly IMongoCollection<ExcludedItems> _excludedItemsCollection;
    private readonly IMongoCollection<Favorites> _favoritesCollection;
    private readonly IMongoCollection<GoogleMapsAddress> _googleMapsAddressCollection;
    private readonly IMongoCollection<Images> _imagesCollection;
    private readonly IMongoCollection<IncludedItems> _includedItemsCollection;
    private readonly IMongoCollection<Ratings> _ratingsCollection;
    private readonly IMongoCollection<Entities.Service> _serviceCollection;
    private readonly IMongoCollection<ServicePlan> _servicePlanCollection;
    private readonly IMongoCollection<TripPlan> _tripPlanCollection;
    private readonly IMongoCollection<Role> _roleCollection;
    private readonly IMongoCollection<Faq> _faqCollection;
    private readonly IMongoCollection<FaqGroup> _faqGroupCollection;
    private readonly IMongoCollection<Page> _PageCollection;
    private readonly IMongoCollection<PageBanner> _pageBannerCollection;
    private readonly IMongoCollection<PageCard> _pageCardCollection;
    private readonly IMongoCollection<LoyProgram> _loyProgramCollection;
    private readonly IMongoCollection<LoyRewardProgram> _loyRewardProgramCollection;
    private readonly IMongoCollection<LoyRewardRedeem> _loyRewardRedeemCollection;
    private readonly IMongoCollection<LoyRewardProduct> _loyRewardProductCollection;
    private readonly IMongoCollection<Game> _gameCollection;
    private readonly IMongoCollection<GameCategory> _gameCategoryCollection;
    private readonly IMongoCollection<SuggestPlan> _suggestPlanCollection;
    private readonly IMongoCollection<GameRate> _gameRateCollection;
    private readonly IMongoCollection<LoyReward> _loyRewardCollection;
    private readonly IMongoCollection<LoyRewardProgramGame> _loyRewardProgramGameCollection;
    private readonly IMongoCollection<LoyRewardAccumulation> _loyRewardAccumulationCollection;
    private readonly IMongoCollection<LoyAccumulationProgram> _LoyAccumulationProgramCollection;
    private readonly IMongoCollection<LoyNotification> _loyNotificationCollection;
    private readonly IMongoCollection<PlaceTourism> _placeTourismCollection;
    private readonly IMongoCollection<PlaceTourismCategory> _placeTourismCategoryCollection;
    private readonly IMongoCollection<PlaceTourismGroup> _placeTourismGroupCollection;
    private readonly IMongoCollection<NotificationPage> _notificationCollection;

    public DatabaseSeeder(IOptions<DatabaseSettings> options)
    {
        var client = new MongoClient(options.Value.ConnectionString);

        var database = client.GetDatabase(options.Value.DatabaseName);
        _roleCollection = database.GetCollection<Role>("role");
        _accountCollection = database.GetCollection<Account>("account");
        _addressCollection = database.GetCollection<Addresses>("addresses");
        _cateCollection = database.GetCollection<Categories>("categories");
        _commentCollection = database.GetCollection<Comments>("comments");
        _dayItineraryCollection = database.GetCollection<GroupTripPlan>("groupTripPlan");
        _groupTripPlanCollection = database.GetCollection<DetailTripPlan>("detailTripPlan");
        _favoritesCollection = database.GetCollection<Favorites>("favorites");
        _googleMapsAddressCollection = database.GetCollection<GoogleMapsAddress>("googleMapsAddress");
        _imagesCollection = database.GetCollection<Images>("images");
        _includedItemsCollection = database.GetCollection<IncludedItems>("includedItems");
        _excludedItemsCollection = database.GetCollection<ExcludedItems>("excludedItems");
        _ratingsCollection = database.GetCollection<Ratings>("ratings");
        _serviceCollection = database.GetCollection<Entities.Service>("service");
        _servicePlanCollection = database.GetCollection<ServicePlan>("servicePlan");
        _tripPlanCollection = database.GetCollection<TripPlan>("tripPlan");
        _faqCollection = database.GetCollection<Faq>("faq");
        _faqGroupCollection = database.GetCollection<FaqGroup>("faqGroup");
        _PageCollection = database.GetCollection<Page>("page");
        _pageBannerCollection = database.GetCollection<PageBanner>("pageBanner");
        _pageCardCollection = database.GetCollection<PageCard>("pageCard");
        _loyProgramCollection = database.GetCollection<LoyProgram>("loyProgram");
        _loyRewardProgramCollection = database.GetCollection<LoyRewardProgram>("loyRewardProgram");
        _loyRewardRedeemCollection = database.GetCollection<LoyRewardRedeem>("loyRewardRedeem");
        _loyRewardProductCollection = database.GetCollection<LoyRewardProduct>("loyRewardProduct");
        _gameCollection = database.GetCollection<Game>("game");
        _gameCategoryCollection = database.GetCollection<GameCategory>("gameCategory");
        _suggestPlanCollection = database.GetCollection<SuggestPlan>("suggestPlan");
        _gameRateCollection = database.GetCollection<GameRate>("gameRate");
        _loyRewardCollection = database.GetCollection<LoyReward>("loyReward");
        _loyRewardProgramGameCollection = database.GetCollection<LoyRewardProgramGame>("loyRewardProgramGame");
        _loyRewardAccumulationCollection = database.GetCollection<LoyRewardAccumulation>("loyRewardAccumulation");
        _LoyAccumulationProgramCollection = database.GetCollection<LoyAccumulationProgram>("loyAccumulationProgram");
        _loyNotificationCollection = database.GetCollection<LoyNotification>("loyNotification");
        _placeTourismCategoryCollection = database.GetCollection<PlaceTourismCategory>("placeTourismCategory");
        _placeTourismCollection = database.GetCollection<PlaceTourism>("placeTourism");
        _placeTourismGroupCollection = database.GetCollection<PlaceTourismGroup>("placeTourismGroup");
        _notificationCollection = database.GetCollection<NotificationPage>("notificationPage");
    }

    public void Seed()
    {
        // Create data for Role
        var RoleList = new List<Role>
        {
            new Role
            {
                _Id = "64eaf9050367766aeffb8eb4",
                Name = "admin"
            },
            new Role
            {
                _Id = "64eaf9050367766aeffb8eb5",
                Name = "user"
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
                Email = "john.doe@example.com",
                Phone = "123-456-7890",
                Password = "$2b$12$VJQLw/P4FzCZ7.3IKj5xZ.AdnEk1wZMZWNiBeQQ1Xe0PQZ29GL7w2",
                PasswordRT = "",
                RoleId = "64eaf9050367766aeffb8eb4", // Replace with an actual role ID
                RegisterDate = DateTime.Now,
                Avatar = "",
                FullName = "Đình Thọ",
            },
            new Account
            {
                _Id = "656499cccf3a97431ceabc92",
                Email = "jane.smith@example.com",
                Phone = "987-654-3210",
                Password = "$2b$12$hGXU3kJC4Bx6yl6HZjG7aOS7fsy/vp02LhV.uSaH55ovEk2UHzIuq",
                PasswordRT = "",
                RoleId = "64eaf9050367766aeffb8eb4", // Replace with another actual role ID
                RegisterDate = DateTime.Now.AddDays(-1),
                Avatar = "",
                FullName = "Đình Thọ",
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
        var serviceList = new List<Entities.Service>
        {
            new Entities.Service
            {
                _Id = "6564689c2884711f5ded091c",
                name = "Romantic Maldives",
                description =
                    "t is a long established fact that a reader will be distracted by the readable content of a page when looking at its layout. The point of using Lorem Ipsum is that it has a more-or-less normal distribution of letters, as opposed to using 'Content here, content here', making it look like readable English. Many desktop publishing packages and web page editors now use Lorem Ipsum as their default model text, and a search for 'lorem ipsum' will uncover many web sites still in their infancy. Various versions have evolved over the years, sometimes by accident, sometimes on purpose (injected humour and the like).",
                price = 9.99m,
                created = DateTime.Now,
                updated = null,
                qualityView = 1080,
                cateId = "65645f2f2884711f5decfe27" // Replace with an actual category ID
            },
            new Entities.Service
            {
                _Id = "65646aef2884711f5ded092a",
                name = "Basic Subscription",
                description = "Access to basic content",
                price = 4.99m,
                created = DateTime.Now.AddDays(-1),
                updated = DateTime.Now.AddHours(-1),
                qualityView = 720,
                cateId = "65645f2f2884711f5decfe27" // Replace with another actual category ID
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
                servicePlanId = "6564689c2884711f5ded091c" // Replace with an actual service plan ID
            },
            new Addresses
            {
                _Id = "65159c8bbf54a316e45e751d",
                country = "Canada",
                province = "Ontario",
                district = "Toronto",
                destailStreet = "456 Maple Avenue",
                servicePlanId = "65646aef2884711f5ded092a" // Replace with another actual service plan ID
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
                _Id = "6568aa6d156925755d7bd380",
                location = "40.7128,-74.0060", // Example: Latitude, Longitude
                fotmattedAddress = "123 Main St, City, Country",
                addressId = "656471952884711f5ded0b34" // Replace with an actual address ID
            },
            new GoogleMapsAddress
            {
                _Id = "6568aac5156925755d7bd38b",
                location = "34.0522,-118.2437", // Example: Latitude, Longitude
                fotmattedAddress = "456 Maple Ave, Another City, Another Country",
                addressId = "65646aef2884711f5ded092a" // Replace with another actual address ID
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
                url =
                    "https://lh3.google.com/u/0/d/1-q65NNQgo2utInZESbmnRaOESJM8DMeC=w2560-h1271-iv2", // Replace with an actual image URL
                servicePlanId = "6564689c2884711f5ded091c" // Replace with an actual service plan ID
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
                _Id = "65645a7e461c0cc9648e9e10",
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
                servicePlanId = "65646aef2884711f5ded092a" // Replace with another actual service plan ID
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
                description =
                    "Content here, content here', making it look like readable English. Many desktop publishing packages and web page editors now use Lorem Ipsum as their default model text, and a search for 'lorem ipsum' will uncover many web sites still in their infancy. ",
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
                userId = "655f9dca62b255ca9b9dcab5", // Replace with another actual user ID
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
                userId = "655f9dca62b255ca9b9dcab5", // Replace with an actual user ID
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
        var dayItineraryList = new List<GroupTripPlan>
        {
            new GroupTripPlan
            {
                _Id = "656949800fd3d398f9580a8c",
                Name = "Ngày yêu thương",
                CreatedAt = DateTime.Now,
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(1),
                IsExpired = false,
                IsPublic = true,
                UserId = "655f9dca62b255ca9b9dcab5"
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
                NumberDay = 1,
                Name = "Bamos",
                Distance = "5.5 km",
                Lat = 2,
                Lng = 2,
                Photos = "",
                HasExperienced = false,
                Rating = 4.5,
                UserRatingsTotal = 2,
                PlaceId = "",
                GroupTripPlanId = "656949800fd3d398f9580a8c"
            },
        };

        if (_groupTripPlanCollection.CountDocuments(_ => true) == 0)
        {
            _groupTripPlanCollection.InsertMany(detailTripPlanList);
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

        var pageList = new List<Page>
        {
            new Page
            {
                _Id = "636460b82884711f5decfeb1",
                PageName = "Trang chủ",
                PageOrder = 1,
                Type = "slide_banner",
            },
            new Page
            {
                _Id = "636460b82884711f5decfeb2",
                PageName = "Trang danh sách kế hoạch",
                PageOrder = 2,
                Type = "list_plan",
            },
            new Page
            {
                _Id = "636460b82884711f5decfeb3",
                PageName = "Trang chủ",
                PageOrder = 3,
                Type = "slide_partner",
            },
            new Page
            {
                _Id = "636460b82884711f5decfeb4",
                PageName = "Trang chủ",
                PageOrder = 4,
                Type = "slide_CTA",
            }
            //new Page
            //{
            //    _Id = "636460b82884711f5decfp002",
            //   PageName = "Trang chủ",
            //   PageOrder = 2,
            //   Type = "list_plan",
            //},
        };

        if (_PageCollection.CountDocuments(_ => true) == 0)
        {
            _PageCollection.InsertMany(pageList);
        }

        var pageBannerList = new List<PageBanner>
        {
            new PageBanner
            {
                _Id = "636460b82884711f5decfeb5",
                PageOrder = 1,
                Action = "brand",
                ActionName = "Đến ngay nào",
                ActionParams = "",
                AllowAllOutlet = 1,
                CreatedAt = DateTime.Now,
                CreatedBy = 1,
                IsActive = true,
                IsDeleted = false,
                PageId = "636460b82884711f5decfeb1",
                PageImg = "",
                PageTagline = "",
                PageTitle = "",
                PageType = "",
                StartEffectiveDate = DateTime.Now,
                EndEffectiveDate = DateTime.Now,
                UpdatedAt = DateTime.Now,
                UpdatedBy = 1,
            },
        };

        if (_pageBannerCollection.CountDocuments(_ => true) == 0)
        {
            _pageBannerCollection.InsertMany(pageBannerList);
        }

        var pageCardList = new List<PageCard>
        {
            new PageCard
            {
                _Id = "636460b82884711f5decfeb6",
                Action = "brand",
                ActionParams = "",
                AllowAllOutlet = 1,
                CreatedAt = DateTime.Now,
                CreatedBy = 1,
                IsActive = true,
                IsDeleted = false,
                StartEffectiveDate = DateTime.Now,
                EndEffectiveDate = DateTime.Now,
                UpdatedAt = DateTime.Now,
                UpdatedBy = 1,
                PageCardImage = "",
                PageCardPosition = 1,
                PageCardTagline = "",
                PageCardTitle = ""
            },
        };

        if (_pageCardCollection.CountDocuments(_ => true) == 0)
        {
            _pageCardCollection.InsertMany(pageCardList);
        }

        var loyProgramList = new List<LoyProgram>
        {
            new LoyProgram
            {
                _Id = "636460b82884711f5decfeb6",
                Name = "Chương trình A",
                Description = "Mô tả chương trình A",
                Periodically = 30,
                PeriodicallyType = "day",
                NameAccumulationPoint = "Điểm thưởng A",
                NameAvailablePoint = "Điểm tiêu A",
                IsActive = true,
                IsDeleted = false,
                CreatedAt = DateTime.Now.AddMonths(-1),
                CreatedBy = 1,
                UpdatedAt = DateTime.Now,
                UpdatedBy = 1
            },
        };

        if (_loyProgramCollection.CountDocuments(_ => true) == 0)
        {
            _loyProgramCollection.InsertMany(loyProgramList);
        }

        var loyRewardProgramList = new List<LoyRewardProgram>
        {
            new LoyRewardProgram
            {
                _Id = "636460b82884711f5decfea1",
                RewardProgramType = "program",
                ProgramId = "636460b82884711f5decfeb6",
                SourcePointKey = "standard",
                RewardProgramName = "Chương trình A",
                RewardProgramDescription = "Mô tả chương trình A",
                RewardProgramCode = "A123",
                Banner = "/images/bannerA.png",
                DateStart = DateTime.Now.AddDays(-10),
                DateEnd = DateTime.Now.AddMonths(1),
                Point = 100,
                RewardId = "501",
                ConfigTurn = "minus_point_directly",
                RewardExpiredType = "day",
                RewardExpiredValue = "30",
                RewardExpiredDate = DateTime.Now.AddMonths(1),
                QuotaChange = 1,
                IsLimitedExchangeAll = true,
                LimitedExchangeType = "rank",
                QuotaLimitedExchangeAll = 100,
                IsLimitedExchangeOutlet = false,
                QuotaLimitedExchangeOutlet = "N/A",
                MembershipType = "all",
                Value = "50000",
                Background = "/images/bgA.png",
                IsActive = true,
                IsDisplay = true,
                IsDeleted = false,
                CreatedAt = DateTime.Now.AddMonths(-1),
                CreatedBy = 1,
                UpdatedAt = DateTime.Now,
                UpdatedBy = 1,
                ApplyLoyalty = true,
                BonusPlaysType = "program",
                PlayTurnNumber = 5
            },
        };

        if (_loyRewardProgramCollection.CountDocuments(_ => true) == 0)
        {
            _loyRewardProgramCollection.InsertMany(loyRewardProgramList);
        }

        var loyRewardRedeemList = new List<LoyRewardRedeem>
        {
            new LoyRewardRedeem
            {
                _Id = "636460b82884711f5decfeb1",
                ProgramId = "636460b82884711f5decfeb6",
                RewardProgramId = "636460b82884711f5decfea1",
                RewardCode = "RC123",
                RewardObjectId = "1001",
                UserId = 1,
                OutletId = "636460b82884711f5decfea1",
                CustomerCode = "CUST001",
                ShipToCode = "STC001",
                Times = 1,
                Point = 500,
                Quota = 2,
                ExpirationDate = DateTime.Now.AddMonths(1),
                IsUsed = false,
                DateUsed = null,
                CreatedAt = DateTime.Now.AddDays(-5),
                CreatedBy = 1,
                UpdatedAt = DateTime.Now,
                UpdatedBy = 1,
                QuantityUnit = 5,
                Status = "pending",
                DeliveryStatus = "not delivered",
                UseAt = null,
                ConfirmedAt = null,
                ForceConfirmed = false,
                QuantityExchange = 2,
                RedeemSource = "app"
            }
        };

        if (_loyRewardRedeemCollection.CountDocuments(_ => true) == 0)
        {
            _loyRewardRedeemCollection.InsertMany(loyRewardRedeemList);
        }

        var loyRewardProductList = new List<LoyRewardProduct>
        {
            new LoyRewardProduct
            {
                _Id = "636460b82884711f5decfec1",
                ProgramId = "636460b82884711f5decfec1",
                RewardId = "RC123",
                RewardProgramId = "636460b82884711f5decfea1",
                ProductId = "636460b82884711f5decfea1",
                ProductUomId = "636460b82884711f5decfea1",
                Quota = 10,
                CreatedAt = DateTime.Now.AddDays(-10),
                CreatedBy = 1,
                UpdatedAt = DateTime.Now.AddDays(-5),
                UpdatedBy = 1
            },
        };

        if (_loyRewardProductCollection.CountDocuments(_ => true) == 0)
        {
            _loyRewardProductCollection.InsertMany(loyRewardProductList);
        }

        var gameCategoryList = new List<GameCategory>
        {
            new GameCategory
            {
                _Id = "636460b82884711f5decfea1",
                CateType = "program",
                CateName = "Vòng xoay may mắn"
            },
        };

        if (_gameCategoryCollection.CountDocuments(_ => true) == 0)
        {
            _gameCategoryCollection.InsertMany(gameCategoryList);
        }

        var gameList = new List<Game>
        {
            new Game
            {
                _Id = ObjectId.GenerateNewId().ToString(),
                RewardProgramId = "636460b82884711f5decfea1",
                CateId = "636460b82884711f5decfea1",
                CateType = "Action",
                GameName = "Spin the Wheel",
                GameCode = "STW2023",
                Banner = "banner1.png",
                Description = "Spin the wheel and win rewards!",
                Intro = "A fun game where you can spin to win!",
                PeriodType = "unlimited",
                StartDate = DateTime.Parse("2024-01-01"),
                EndDate = DateTime.Parse("2024-12-31"),
                Frequency = "daily",
                FrequencyValue = "", // Every day
                FrequencyMonthlyType = "",
                DayInMonthly = "",
                DayInWeek = "",
                DayInWeekRepeat = "",
                PeriodInDateType = "unlimited",
                PeriodInDateStart = "",
                PeriodInDateEnd = "",
                ConfigTurn = "minus_points_directly",
                WinType = true,
                WinQuotaLimit = -1, // No limit
                NumbetWheel = 8,
                ObjectApply = "all",
                IsUpdateInfo = true,
                IsUpdateWin = true,
                IsActived = true,
                IsActive = true,
                IsDeleted = false,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = 1,
                UpdatedAt = DateTime.UtcNow,
                UpdatedBy = 1,
                ApplyTotalBudget = true
            },
        };

        if (_gameCollection.CountDocuments(_ => true) == 0)
        {
            _gameCollection.InsertMany(gameList);
        }

        var suggestPlanList = new List<SuggestPlan>
        {
            new SuggestPlan
            {
                _Id = ObjectId.GenerateNewId().ToString(),
                Name = "Ăn sáng",
                Order = 1,
                Type = "7AM-9AM",
                UpdatedAt = DateTime.UtcNow,
                UpdatedBy = 1,
            },
            new SuggestPlan
            {
                _Id = ObjectId.GenerateNewId().ToString(),
                Name = "Địa điểm vui chơi (buổi sáng)",
                Order = 2,
                Type = "9AM-11AM",
                UpdatedAt = DateTime.UtcNow,
                UpdatedBy = 1,
            },
            new SuggestPlan
            {
                _Id = ObjectId.GenerateNewId().ToString(),
                Name = "Ăn trưa",
                Order = 3,
                Type = "11PM-1PM",
                UpdatedAt = DateTime.UtcNow,
                UpdatedBy = 1,
            },
            new SuggestPlan
            {
                _Id = ObjectId.GenerateNewId().ToString(),
                Name = "Địa điểm vui chơi 1 (buổi chiều)",
                Order = 4,
                Type = "1PM-3PM",
                UpdatedAt = DateTime.UtcNow,
                UpdatedBy = 1,
            },
            new SuggestPlan
            {
                _Id = ObjectId.GenerateNewId().ToString(),
                Name = "Địa điểm vui chơi 2 (buổi chiều)",
                Order = 5,
                Type = "3PM-5PM",
                UpdatedAt = DateTime.UtcNow,
                UpdatedBy = 1,
            },
            new SuggestPlan
            {
                _Id = ObjectId.GenerateNewId().ToString(),
                Name = "Ăn tối",
                Order = 6,
                Type = "5PM-7PM",
                UpdatedAt = DateTime.UtcNow,
                UpdatedBy = 1,
            },
            new SuggestPlan
            {
                _Id = ObjectId.GenerateNewId().ToString(),
                Name = "Địa điểm vui chơi (buổi tối)",
                Order = 7,
                Type = "7PM-9PM",
                UpdatedAt = DateTime.UtcNow,
                UpdatedBy = 1,
            },
        };

        if (_suggestPlanCollection.CountDocuments(_ => true) == 0)
        {
            _suggestPlanCollection.InsertMany(suggestPlanList);
        }

        var gameRateList = new List<GameRate>
        {
            new GameRate()
            {
                _Id = ObjectId.GenerateNewId().ToString(),
                GameId = "66f926f11f4d8fa281acd3d1",
                PercentWin = 100,
                Times = -1
            },
        };

        if (_gameRateCollection.CountDocuments(_ => true) == 0)
        {
            _gameRateCollection.InsertMany(gameRateList);
        }

        var loyRewardList = new List<LoyReward>
        {
            new LoyReward()
            {
                _Id = "66f926f11f4d8fa281acd3d2",
                Name = "discount",
                CreatedAt = DateTime.UtcNow,
                Gift = "",
                Icon = "",
                GameWheel = true,
                IsWin = true
            },
            new LoyReward()
            {
                _Id = "66f926f11f4d8fa281acd3d3",
                Name = "point",
                CreatedAt = DateTime.UtcNow,
                Gift = "",
                Icon = "",
                GameWheel = true,
                IsWin = true
            },
            new LoyReward()
            {
                _Id = "66f926f11f4d8fa281acd3d4",
                Name = "not_win",
                CreatedAt = DateTime.UtcNow,
                Gift = "",
                Icon = "",
                GameWheel = true,
                IsWin = true
            },
        };

        if (_loyRewardCollection.CountDocuments(_ => true) == 0)
        {
            _loyRewardCollection.InsertMany(loyRewardList);
        }

        var loyRewardProgramGameList = new List<LoyRewardProgramGame>
        {
            new LoyRewardProgramGame()
            {
                _Id = "66f926f11f4d8fa281acd3d2",
                RewardProgramId = "66f926f11f4d8fa281acd3d1",
                GameId = "66f926f11f4d8fa281acd3d1",
                CreatedAt = DateTime.Now,
                Position = 1,
                WinRate = 2,
                TotalBudgetQuota = 200,
                IsDisplay = true,
                UpdatedAt = DateTime.Now,
                Background = "",
                IsWin = true,
                Image = "",
                Value = "",
                QuantityUnit = 200,
            },
            new LoyRewardProgramGame()
            {
                _Id = "66f926f11f4d8fa281acd3d4",
                RewardProgramId = "66f926f11f4d8fa281acd3d1",
                GameId = "66f926f11f4d8fa281acd3d1",
                CreatedAt = DateTime.Now,
                Position = 1,
                WinRate = 2,
                TotalBudgetQuota = 200,
                IsDisplay = true,
                UpdatedAt = DateTime.Now,
                Background = "",
                IsWin = true,
                Image = "",
                Value = "",
                QuantityUnit = 200,
            },
        };

        if (_loyRewardProgramGameCollection.CountDocuments(_ => true) == 0)
        {
            _loyRewardProgramGameCollection.InsertMany(loyRewardProgramGameList);
        }

        var loyAccumulationProgramList = new List<LoyAccumulationProgram>
        {
            new LoyAccumulationProgram()
            {
                _Id = "66f926f11f4d8fa281acd3d2",
                ObjId = "",
                CreatedAt = DateTime.Now,
                AccumulationPoint = 200,
                ProgramId = "636460b82884711f5decfeb6",
                UpdatedAt = DateTime.Now,
                IsDeleted = false,
                CreatedBy = 1,
                UpdatedBy = 1,
                IsActive = true,
                SourcePointKey = "game_wheel",
                ApplyType = "all",
                AllowAllOutlet = 1,
                AccumulationProgramCode = "AP123",
                AccumulationProgramName = "Nhân đôi số điểm",
                DateStart = DateTime.Now,
                AvailablePoint = 1,
                DateEnd = DateTime.Now.AddMonths(1),
                BudgetConfig = "minus_points_directly",
                EnableBudget = 1,
                AccumulationProgramResultType = "all",
                FailAccumulationPoint = 1,
                ExtraConfig = "all",
                FailAvailablePoint = 1,
                ValidityPeriodType = ""
            },
        };

        if (_LoyAccumulationProgramCollection.CountDocuments(_ => true) == 0)
        {
            _LoyAccumulationProgramCollection.InsertMany(loyAccumulationProgramList);
        }

        var loyRewardAccumulationList = new List<LoyRewardAccumulation>
        {
            new LoyRewardAccumulation()
            {
                _Id = "66f926f11f4d8fa281acd3d4",
                RewardProgramId = "66f926f11f4d8fa281acd3d2",
                Quota = 100,
                RewardId = "66f926f11f4d8fa281acd3d2",
                CreatedAt = DateTime.Now,
                CreatedBy = 1,
                UpdatedAt = DateTime.Now,
                UpdatedBy = 1,
                ProgramId = "636460b82884711f5decfeb6",
                AccumulationProgramId = "66f926f11f4d8fa281acd3d2"
            },
        };

        if (_loyRewardAccumulationCollection.CountDocuments(_ => true) == 0)
        {
            _loyRewardAccumulationCollection.InsertMany(loyRewardAccumulationList);
        }

        var loyNotificationList = new List<LoyNotification>
        {
            new LoyNotification()
            {
                _Id = "66f926f11f4d8fa281acd3d4",
                Title = "Thông báo 1",
                Description = "Nội dung thông báo 1",
                CreatedAt = DateTime.Now,
                CreatedBy = 1,
                UpdatedAt = DateTime.Now,
                UpdatedBy = 1,
                Background = "",
                TitleShow = "",
                DescriptionShow = "",
                IsAction = 1,
                ObjectId = "",
                Key = "",
                SubId = 1,
                IsNotify = 1,
                NotificationType = "",
                ParamsShow = "",
                ObjectSubId = ""
            },
        };

        if (_loyNotificationCollection.CountDocuments(_ => true) == 0)
        {
            _loyNotificationCollection.InsertMany(loyNotificationList);
        }

        var placeTourismCategoryList = new List<PlaceTourismCategory>
        {
            new PlaceTourismCategory()
            {
                _Id = "66f926f11f4d8fa281acd3d4",
                Name = "Khách sạn",
                Type = "hotel",
                CreatedAt = DateTime.Now,
                CreatedBy = 1,
                UpdatedAt = DateTime.Now,
                UpdatedBy = 1,
            },
        };

        if (_placeTourismCategoryCollection.CountDocuments(_ => true) == 0)
        {
            _placeTourismCategoryCollection.InsertMany(placeTourismCategoryList);
        }

        var placeTourismGroupList = new List<PlaceTourismGroup>
        {
            new PlaceTourismGroup()
            {
                _Id = "66f926f11f4d8fa281acd3d4",
                PlaceTourismCateId = "66f926f11f4d8fa281acd3d4",
                Name = "Khách sạn Luxury Chánh Kiệt",
                Description = "hotel",
                CreatedAt = DateTime.Now,
                CreatedBy = 1,
                UpdatedAt = DateTime.Now,
                UpdatedBy = 1,
                IsActive = true,
                IsDeleted = false
            },
        };

        if (_placeTourismGroupCollection.CountDocuments(_ => true) == 0)
        {
            _placeTourismGroupCollection.InsertMany(placeTourismGroupList);
        }

        var placeTourismList = new List<PlaceTourism>
        {
            new PlaceTourism()
            {
                _Id = "66f926f11f4d8fa281acd3d4",
                Name = "Khách sạn Luxury Chánh Kiệt chi nhánh 1",
                PlaceTourismGroupId = "66f926f11f4d8fa281acd3d4",
                Latitude = "",
                Longitude = "",
                Description = "hotel",
                Location = "Đà Làt",
                Rating = 4.8,
                MinEntryFee = 100,
                MaxEntryFee = 200,
                ClosingTime = "8 AM",
                OpeningTime = "20 PM",
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                IsActive = true,
                IsDeleted = false
            },
        };

        if (_placeTourismCollection.CountDocuments(_ => true) == 0)
        {
            _placeTourismCollection.InsertMany(placeTourismList);
        }
        
        var notificationList = new List<NotificationPage>
        {
            new NotificationPage()
            {
               _Id = "66f926f11f4d8fa281acd3d4",
               Title = "Trang chi tiết game",
               Type = "game",
               IsActive = true,
               Description = "Nội dung thông báo 1",
               StartDate = DateTime.Now,
               EndDate = DateTime.Now,
               PageId = "636460b82884711f5decfeb4",
               
            },
        };

        if (_notificationCollection.CountDocuments(_ => true) == 0)
        {
            _notificationCollection.InsertMany(notificationList);
        }
    }
    
    
    
}
    