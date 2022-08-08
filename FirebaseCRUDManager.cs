using System.Collections;
using UnityEngine;
using Firebase;
using Firebase.Database;
using Firebase.Auth;
using TMPro;

public class FirebaseCRUDManager : MonoBehaviour
{
    //Login Input fields 
    public TMP_InputField emailLoginField;
    public TMP_InputField passwordLoginField;

    //Register Input fields
    public TMP_InputField emailRegisterField;
    public TMP_InputField passwordRegisterField;
    public TMP_InputField passwordRegisterVerifyField;

    //Reset Input fields
    public TMP_InputField emailResetField;

    [HideInInspector]
    public Firebase.Auth.FirebaseAuth auth = null;
    [HideInInspector]
    public Firebase.Auth.FirebaseUser user = null;
    [HideInInspector]
    public DatabaseReference DatabaseInstance;

    bool signedIn = false;
    public GameObject userMetadataGameObject;
    public GameObject trailblazerCanvas;
    public GameObject loginSystem;
    public notificationManager notificiantionManager;
    public GameObject LoginPanel;
    public GameObject RegisterPanel;

    // --------- Firebase Initialization ---------  //
    // --------- 1 ---------  //
    private void Awake()
    { 
        //start the corutine
        StartCoroutine(CheckAndFixDependenciesCoroutine());
    }

    // --------- 2 ---------  //
    private IEnumerator CheckAndFixDependenciesCoroutine()
    {
        //hold the task in a temporal variable
        var checkDependenciesTask = Firebase.FirebaseApp.CheckAndFixDependenciesAsync();
        // wait until the task is completed
        yield return new WaitUntil(() => checkDependenciesTask.IsCompleted);
        //save the result of the task in a temporal variable
        var dependencyStatus = checkDependenciesTask.Result;
        //if the result of the task is equal to available
        if (dependencyStatus == Firebase.DependencyStatus.Available)
        {
            //then say Firebase available and display a smile face
            Debug.Log($"Firebase: {dependencyStatus} :)");
            //then initialize the Firebase Auth
            InitializeFirebaseAuthUser();
            //finally initialize the database
            InitializeFirebaseDB();
        }
        else
        {
            Debug.LogError(System.String.Format("Could not resolve all Firebase dependencies: {0}", dependencyStatus));
            // Firebase Unity SDK is not safe to use here.
        }
    }
    
    // --------- 4 ---------  //
    void AuthStateChanged(object sender, System.EventArgs eventArgs) {
         Debug.Log("start eh");
         signedIn = user != auth.CurrentUser && auth.CurrentUser != null;
        if (auth.CurrentUser != user) 
        {
            if (!signedIn && user != null) 
            {
                Debug.Log("Signed out ");
            }
        user = auth.CurrentUser;
        }
    }

    // --------- 5 ---------  //
    public void checkState(){
        if (signedIn){
            print("is signed in");
             Debug.Log("Signed in " + user.UserId);
             if(user.IsEmailVerified){
                 print("email is verified");
                 logOut();
                //script.loadSceneOne();
             } else {
                print("email is not verified");
                logOut();
             }
        }
        if(!signedIn){
             print("is not signed in");
             //script.loadSceneOne();
        }
    }
    // --------- 6 ---------  //
    public void logOut()
    {
        auth.SignOut();
    }

    // --------- 3 ---------  //
    void InitializeFirebaseAuthUser() {
        auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
        auth.StateChanged += AuthStateChanged;
        AuthStateChanged(this, null);
        checkState();
    }


    private void InitializeFirebaseDB()
    {
        DatabaseInstance = FirebaseDatabase.DefaultInstance.RootReference;
    }

    // --------- Methods of the Firebase CRUD Manager class ---------  //

    //Login function for when the send button is pressed
    public void LoginButton()
    {
        //Call the login coroutine passing the email and password to login the user
        StartCoroutine(Login(emailLoginField.text, passwordLoginField.text));
    }

    //Corutine for login using Firebase Auth
    private IEnumerator Login(string _email, string _password)
    {
        //Call the Firebase auth signin function passing the email and password
        var LoginTask = auth.SignInWithEmailAndPasswordAsync(_email, _password);
        //Wait until the task completes
        yield return new WaitUntil(predicate: () => LoginTask.IsCompleted);

        if (LoginTask.Exception != null)
        {
            
            Debug.LogWarning(message: $"Failed to register task with {LoginTask.Exception}");
            FirebaseException firebaseEx = LoginTask.Exception.GetBaseException() as FirebaseException;
            AuthError errorCode = (AuthError)firebaseEx.ErrorCode;

            string message;
            switch (errorCode)
            {
                case AuthError.MissingEmail:
                    message = "Missing Email";
                    break;
                case AuthError.MissingPassword:
                    message = "Missing Password";
                    break;
                case AuthError.WrongPassword:
                    message = "Wrong Password";
                    break;
                case AuthError.InvalidEmail:
                    message = "Invalid Email";
                    break;
                case AuthError.UserNotFound:
                    message = "Account does not exist";
                    break;
            }
            print("Login unsuccessfull");
             LoginPanel.SetActive(false);
             //show notification
             notificiantionManager.tittleText = "Login unsuccessfully";
             notificiantionManager.bodyText = "try Again";
             notificiantionManager.changeAndOpen();
        }
        else
        {
            user = LoginTask.Result;

            if (user != null) 
            {
                print("User is not null");
                
                if(user.IsEmailVerified)
                {
                    print("email is verified");
                    //getSetUserScripts();
                    // StartCoroutine(loadUserProfile(user.UserId));
                   // print(uscript.userID);
                    loginSystem.SetActive(false);
                    trailblazerCanvas.SetActive(true);
                } 
                else 
                {
                    print("email is not verified");
                    LoginPanel.SetActive(false);
                    notificiantionManager.tittleText = "Email not verified ";
                    notificiantionManager.bodyText = "Verify email and try again";
                    notificiantionManager.changeAndOpen();
                 //   lscene.loadSceneOnePointFive();
                }
            }
           
            Debug.LogFormat("User signed in successfully: {0} ({1})", user.DisplayName, user.Email);
            var uidx = user.UserId;
            print("user ID :  " + uidx); 
            
        }
    }

    //Register function for when the send button is pressed
    public void RegisterButton()
    {
        //Call the Register coroutine passing the email and password to register the user
        StartCoroutine(Register(emailRegisterField.text, passwordRegisterVerifyField.text));
    }

    public IEnumerator setUID(string uid)
    {
        var DBTask = DatabaseInstance.Child("accounts").Child(user.UserId).Child("uid").SetValueAsync(uid);
        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if(DBTask.Exception != null)
        {
            print("error");
        } else {
            print("uid set to :" + uid);
        }
    }

    public IEnumerator setEmail(string email)
    {
        var DBTask = DatabaseInstance.Child("accounts").Child(user.UserId).Child("email").SetValueAsync(email);
        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if(DBTask.Exception != null)
        {
            print("error while setting email");
        } else {
            print("email set to :" + email);
        }
    }

    
    public void verifyAccount(){
        Firebase.Auth.FirebaseUser user = auth.CurrentUser;
        if (user != null) {
            user.SendEmailVerificationAsync().ContinueWith(task => {
                if (task.IsCanceled) {
                Debug.LogError("SendEmailVerificationAsync was canceled.");
                return;
                }
                if (task.IsFaulted) {
                Debug.LogError("SendEmailVerificationAsync encountered an error: " + task.Exception);
                return;
                }
                notificiantionManager.tittleText = "Email verification sent 😀";
                notificiantionManager.bodyText = "Verify email and try again";
                notificiantionManager.changeAndOpen();
            });
        }
    }
    
    public void setDataToFirebase(string emailSet, string uidSet)
    {
       StartCoroutine(setEmail(emailSet));
       StartCoroutine(setUID(uidSet));
    }
    
    public void resetPassword(){
        StartCoroutine(ResetPassword(emailResetField.text));
    }

    private IEnumerator ResetPassword(string email){
        if(email == ""){
             notificiantionManager.tittleText = "Missing email 😮";
             notificiantionManager.bodyText = "email field is empty";
             notificiantionManager.changeAndOpen();
        } else {
            var ResetTask = auth.SendPasswordResetEmailAsync(email);
            yield return new WaitUntil(predicate: () => ResetTask.IsCompleted);
            if(ResetTask.Exception != null)
            {
                notificiantionManager.tittleText = "Ups! Errors on our side..";
                notificiantionManager.bodyText = $"Error {ResetTask.Exception}";
                notificiantionManager.changeAndOpen();
            } else {
                print("email set to :" + email);
                notificiantionManager.tittleText = "Reset sucessful 🤩";
                notificiantionManager.bodyText = "Check your spam folder!";
                notificiantionManager.changeAndOpen();
            }
        }
    }

    //Corutine for register using Firebase Auth
    private IEnumerator Register(string _email, string _password)
    {
        if (_email == "")
        {
            //if the user input field is empty
            //show notification
             notificiantionManager.tittleText = "Missing email 😮";
             notificiantionManager.bodyText = "email field is empty";
             notificiantionManager.changeAndOpen();
        }
        else if(passwordRegisterField.text != passwordRegisterVerifyField.text)
        {
            //If the passwords dont match
            //show notification
             notificiantionManager.tittleText = "Paswords dont match";
             notificiantionManager.bodyText = "Make sure both passwords match";
             notificiantionManager.changeAndOpen();
        }
        else 
        {
            //Call the Firebase auth register method and pass the email and password
            var RegisterTask = auth.CreateUserWithEmailAndPasswordAsync(_email, _password);
            //Wait until the task completes
            yield return new WaitUntil(predicate: () => RegisterTask.IsCompleted);

            if (RegisterTask.Exception != null)
            {
                //If there are errors handle them and
                Debug.LogWarning(message: $"Failed to register task with {RegisterTask.Exception}");
                //show notification
                notificiantionManager.tittleText = "Ups! Errors on our side..";
                notificiantionManager.bodyText = $"Error {RegisterTask.Exception}";
                notificiantionManager.changeAndOpen();
                FirebaseException firebaseEx = RegisterTask.Exception.GetBaseException() as FirebaseException;
                AuthError errorCode = (AuthError)firebaseEx.ErrorCode;

                string message = "Register Failed!";
                switch (errorCode)
                {
                    case AuthError.MissingEmail:
                        message = "Missing Email";
                        break;
                    case AuthError.MissingPassword:
                        message = "Missing Password";
                        break;
                    case AuthError.WeakPassword:
                        message = "Weak Password";
                        break;
                    case AuthError.EmailAlreadyInUse:
                        message = "Email Already In Use";
                        break;
                }

                    notificiantionManager.tittleText = "We found an error on our side";
                    notificiantionManager.bodyText = message;
                    notificiantionManager.changeAndOpen();
            }
            else
            {
                user = RegisterTask.Result;
                if (user != null)
                {
                    //Create a user profile and set the username
                     UserProfile profile = new UserProfile{DisplayName = _email};
                    //Call the Firebase auth update user profile function passing the profile with the username
                    var ProfileTask = user.UpdateUserProfileAsync(profile);
                    //Wait until the task completes
                    yield return new WaitUntil(predicate: () => ProfileTask.IsCompleted);

                    if (ProfileTask.Exception != null)
                    {
                        //If there are errors handle them
                        Debug.LogWarning(message: $"Failed to register task with {ProfileTask.Exception}");
                        FirebaseException firebaseEx = ProfileTask.Exception.GetBaseException() as FirebaseException;
                        AuthError errorCode = (AuthError)firebaseEx.ErrorCode;
                       // warningRegisterText.text = "Username Set Failed!";

                        //show notification
                        notificiantionManager.tittleText = "Failed to register";
                        notificiantionManager.bodyText = $"Failed to register task with {ProfileTask.Exception}";
                        notificiantionManager.changeAndOpen();
                    }
                    else
                    {
                        notificiantionManager.tittleText = "User registered succesfully";
                        notificiantionManager.bodyText = "Email verification sent";
                        //clean input fields before closing the screen
                        emailRegisterField.text = "";
                        passwordRegisterField.text = "";
                        passwordRegisterVerifyField.text = "";
                        //turn off screen register
                        RegisterPanel.SetActive(false);
                        // set to the firebase real time database the user and email under the UID of the user
                        setDataToFirebase( _email, user.UserId);
                        notificiantionManager.changeAndOpen();
                        //send notification of verify email 
                        verifyAccount();
                    }
                }
            }
        }
    }
}
