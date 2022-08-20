// using System;
// using System.IO;
// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.UI;
// using Firebase.Storage;
// using System.Threading.Tasks;
// using Firebase.Extensions;

// public class ImageCommunication : MonoBehaviour
// {
//     //gs://<your-cloud-storage-bucket>/<sub_path>/
//     public string m_Path = "gs://<your-cloud-storage-bucket>/images/";
//     public string m_FilePrefix = "Capture";   //fileName
//     private string m_FilePath;
    
//     public void UploadImage()
//     {
//         // Create a reference to the file you want to upload
//         m_FilePath = m_Path + m_FilePrefix + DateTime.Now.ToString("yyyyMMddhhmmss") + ".jpg";
//         StartCoroutine(SaveScreeJpg(m_FilePath));
//     }

//     IEnumerator SaveScreeJpg(string filePath)
//     {
//         yield return new WaitForEndOfFrame();

//         // Start is called before the first frame update
//         FirebaseStorage storage = FirebaseStorage.DefaultInstance;
//         // Update is called once per frame
//         StorageReference spaceRefFull = storage.GetReferenceFromUrl(filePath);

//         Texture2D texture = new Texture2D(Screen.width, Screen.height / 2);
//         texture.ReadPixels(new Rect(0, 750, Screen.width, Screen.height / 2), 0, 0);    // Rect(x축 시작 위치, y축 시작 위치)   // ReadPixels(원본, x축 어디, y축 어디)
//         texture.Apply();
        
//         // Data in memory
//         byte[] bytes = texture.EncodeToJPG();   // jpg로 변환
        
//         spaceRefFull.PutBytesAsync(bytes)
//             .ContinueWith((Task<StorageMetadata> task) => {
//                 if (task.IsFaulted || task.IsCanceled) {
//                     Debug.Log(task.Exception.ToString());
//                     // Uh-oh, an error occurred!
//                 }
//                 else {
//                     // Metadata contains file metadata such as size, content-type, and md5hash.
//                     StorageMetadata metadata = task.Result;
//                     string md5Hash = metadata.Md5Hash;
//                     Debug.Log("Finished uploading...");
//                     Debug.Log("md5 hash = " + md5Hash);
//                 }
//             });

//         DestroyImmediate(texture);
//     }
// }