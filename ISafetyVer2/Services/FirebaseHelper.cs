using Firebase.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISafetyVer2.Models;
using Firebase.Database.Query;
using Firebase.Storage;
using Firebase.Auth;

namespace ISafetyVer2.Services
{
    internal class FirebaseHelper
    {
        // For Realtime database:

        public FirebaseClient firebaseClient = new FirebaseClient(baseUrl: Settings.FireBaseDatabaseURL);

        // Add User:
        public async Task<string> AddUser(DBUser user)
        {
            FirebaseObject<DBUser> result = await firebaseClient.Child("Users").PostAsync(user);
            return result.Key;
        }

        // Get User by UserID:
        public async Task<DBUser> GetDBUserByUserID(string userID)
        {
            IReadOnlyCollection<FirebaseObject<DBUser>> result = await firebaseClient
                .Child("Users")
                .OrderBy("UserID")
                .EqualTo(userID)
                .OnceAsync<DBUser>();

            return result.FirstOrDefault()?.Object; // Return null if there are no matches.
        }

        // Add Category:
        public async Task<string> AddCategory(Category category)
        {
            FirebaseObject<Category> result = await firebaseClient.Child("Categories").PostAsync(category);
            return result.Key;
        }

        // Get all Categories:
        public async Task<List<Category>> GetAllCategories()
        {
            return (await firebaseClient.Child("Categories").OnceAsync<Category>()).Select(item => new Category
            {
                CategoryID = item.Key,
                CategoryName = item.Object.CategoryName
            }).ToList();
        }

        // Add SubCategory:
        public async Task<string> AddSubCategory(SubCategory subcategory)
        {
            FirebaseObject<SubCategory> result = await firebaseClient.Child("Subcategories").PostAsync(subcategory);
            return result.Key;
        }

        // Get all SubCategories:
        public async Task<List<SubCategory>> GetAllSubCategories()
        {
            return (await firebaseClient.Child("Subcategories").OnceAsync<SubCategory>()).Select(item => new SubCategory
            {
                SubCatID = item.Key,
                CategoryID = item.Object.CategoryID,
                SubCatName = item.Object.SubCatName,
                DangerLvl = item.Object.DangerLvl,
                SafetyTipsDescription = item.Object.SafetyTipsDescription
            }).ToList();
        }

        // Get SubCategory by SubCatID:
        public async Task<SubCategory> GetSubCategoryBySubCatID(string subCatID)
        {
            IReadOnlyCollection<FirebaseObject<SubCategory>> result = await firebaseClient
                .Child("Subcategories")
                .OrderByKey()
                .EqualTo(subCatID)
                .OnceAsync<SubCategory>();

            return result.FirstOrDefault()?.Object; // Return null if there are no matches.
        }

        // Get SubCategory by CategoryID:
        /*
        // This will download all subcategories, might need to change it later (maybe using similar approach with GetDBUserByUserID()).
        public async Task<List<SubCategory>> GetSubCategoriesByCategoryID(string categoryID)
        {
            return (await firebaseClient.Child("Subcategories").OnceAsync<SubCategory>()).Where(item => item.Object.CategoryID == categoryID).Select(item => new SubCategory
            {
                SubCatID = item.Key,
                CategoryID = item.Object.CategoryID,
                SubCatName = item.Object.SubCatName,
                DangerLvl = item.Object.DangerLvl
            }).ToList();
        }
        */
        public async Task<List<SubCategory>> GetSubCategoriesByCategoryID(string categoryID)
        {
            IReadOnlyCollection<FirebaseObject<SubCategory>> result = await firebaseClient
                .Child("Subcategories")
                .OrderBy("CategoryID")
                .EqualTo(categoryID)
                .OnceAsync<SubCategory>();

            return result.Select(item => new SubCategory
            {
                SubCatID = item.Key,
                CategoryID = item.Object.CategoryID,
                SubCatName = item.Object.SubCatName,
                DangerLvl = item.Object.DangerLvl,
                SafetyTipsDescription = item.Object.SafetyTipsDescription
            }).ToList();
        }

        // Add QuickReport:
        public async Task<string> AddQuickReport(QuickReport quickReport)
        {
            FirebaseObject<QuickReport> result = await firebaseClient.Child("QuickReports").PostAsync(quickReport);
            return result.Key;
        }

        // Get all QuickReports with SubCategories and Categories:
        public async Task<List<QuickReport>> GetAllQuickReport()
        {
            return (await firebaseClient.Child("QuickReports").OnceAsync<QuickReport>()).Select(item => new QuickReport
            {
                QRID = item.Key,
                UserID = item.Object.UserID,
                SubCatID = item.Object.SubCatID,
                ReportDateTime = item.Object.ReportDateTime,
                QRDescription = item.Object.QRDescription,
                Latitude = item.Object.Latitude,
                Longitude = item.Object.Longitude,
                Radius = item.Object.Radius,
                MediaURL = item.Object.MediaURL,
                Status = item.Object.Status
            }).ToList();
        }

        // Get QuickReports by UserID:
        public async Task<List<QuickReport>> GetAllQuickReportByUserID(string userID)
        {
            IReadOnlyCollection<FirebaseObject<QuickReport>> result = await firebaseClient
                .Child("QuickReports")
                .OrderBy("UserID")
                .EqualTo(userID)
                .OnceAsync<QuickReport>();

            return result.Select(item => new QuickReport
            {
                QRID = item.Key,
                UserID = item.Object.UserID,
                SubCatID = item.Object.SubCatID,
                ReportDateTime = item.Object.ReportDateTime,
                QRDescription = item.Object.QRDescription,
                Latitude = item.Object.Latitude,
                Longitude = item.Object.Longitude,
                Radius = item.Object.Radius,
                MediaURL = item.Object.MediaURL,
                Status = item.Object.Status
            }).ToList();
        }

        /*-------------------------------------------------------------------------------------------------------------------------------------------------------------*/
        // For Storage:
        public FirebaseStorage firebaseStorage = new FirebaseStorage(Settings.FireBaseStorageBucket);

        // Add media files to firebase storage:
        public async Task<string> UploadMediaToFirebase(string mediaPath)
        {
            FileStream stream = File.OpenRead(mediaPath);   // Open a stream to the file.

            string filename = Path.GetFileName(mediaPath);
            filename = $"{DateTime.Now.ToString("yyyyMMdd_HHmmssfff")}_{filename}"; // Create unique filename.

            string storageMedia = await firebaseStorage
                .Child("Medias")
                .Child(filename)
                .PutAsync(stream);

            stream.Dispose();   // Dispose the stream.

            return storageMedia;    // The URL of the media in firebase storage.
        }

        /*-------------------------------------------------------------------------------------------------------------------------------------------------------------*/
        // General:

        // Get file info using URL:
        //Admin POst
        // In FirebaseHelper.cs
        public async Task<string> AddAdminPost(AdminPost post)
        {
            var result = await firebaseClient.Child("AdminPosts").PostAsync(post);
            return result.Key;  // Returns the unique ID of the new post.
        }


        //For adminpost fetching dataafrom firebaserealtimedatabase
        public async Task<List<AdminPost>> GetAllAdminPosts()
        {
            var result = await firebaseClient
                .Child("AdminPosts")
                .OnceAsync<AdminPost>();

            return result.Select(item => new AdminPost
            {
                PostID = item.Key,
                PostTitle = item.Object.PostTitle,
                ReportDateTime = item.Object.ReportDateTime,
                PostDescription = item.Object.PostDescription,
                Location = item.Object.Location,
                MediaURL = item.Object.MediaURL
            }).ToList();
        }


    }
}
