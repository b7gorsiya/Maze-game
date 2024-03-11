using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using Google.Play.Review;

public class RManger : MonoBehaviour
{
    // Create instance of ReviewManager
    //private ReviewManager _reviewManager;

   // private PlayReviewInfo _playReviewInfo;
    // ...

    public  void OpenInAppReview()
    {
        //StartCoroutine(Open_In_App_Review());
    }
    //IEnumerator Open_In_App_Review()
    //{
    //    Debug.Log("Review box open");
    //  //  _reviewManager = new ReviewManager();

    //    // request review
    //  //  var requestFlowOperation = _reviewManager.RequestReviewFlow();
    //    //yield return requestFlowOperation;
    // /*   requestFlowOperation.Completed  += playReviewInfoAsync =>
    //    {
    //        if (playReviewInfoAsync.Error == ReviewErrorCode.NoError)
    //        {
    //            // display the review prompt
    //            var playReviewInfo = playReviewInfoAsync.GetResult();
    //            _reviewManager.LaunchReviewFlow(playReviewInfo);
    //            Debug.Log("Dsiplay th review dialog");
    //        }
    //        else
    //        {
    //            Debug.Log("Erro " + requestFlowOperation.Error);
    //            // handle error when loading review prompt
    //        }
    //    };
    //   */
    //    //if (requestFlowOperation.Error != ReviewErrorCode.NoError)
    //    //{
    //    //    // Log error. For example, using requestFlowOperation.Error.ToString().
    //    //    Debug.Log(requestFlowOperation.Error);
    //    //    yield break;
    //    //}
    //    //_playReviewInfo = requestFlowOperation.GetResult();


    //    // show review dialog
    //    //var launchFlowOperation = _reviewManager.LaunchReviewFlow(_playReviewInfo);
    //    //yield return launchFlowOperation;
    //    //_playReviewInfo = null; // Reset the object
    //    //if (launchFlowOperation.Error != ReviewErrorCode.NoError)
    //    //{
    //    //    // Log error. For example, using requestFlowOperation.Error.ToString().#
    //    //    Debug.Log(launchFlowOperation.Error);
    //    //    yield break;
    //    //}
    //    //else { PlayerPrefs.SetInt("Rated", 1); }
    //    // The flow has finished. The API does not indicate whether the user
    //    // reviewed or not, or even whether the review dialog was shown. Thus, no
    //    // matter the result, we continue our app flow.
    //}


}
