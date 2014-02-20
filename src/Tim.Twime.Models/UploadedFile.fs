namespace Tim.Twime.Models

//module UploadedFile =

    open System.IO

    type UploadedFile =
        { Filename : string
          Content : Stream }


