﻿using Azure.Core;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Fakebook.Application.Generics.Interfaces;
using Fakebook.Application.Options;
using FakeBook.Domain.Aggregates.PostAggregate;
using FakeBook.Domain.Aggregates.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Fakebook.Application.Services
{
    public class MediaService : IPhotoService, IVideoService, IAudioService 
    {
        private readonly Cloudinary _cloudinary;
        private readonly MediaSettings _mediaSettings;

        public MediaService(IOptions<ColudinarySettings> config, IOptions<MediaSettings> mediaSettings)
        {
            var account = new Account
            (
                config.Value.CloudName,
                config.Value.ApiKey,
                config.Value.ApiSecret
            );
            _cloudinary = new Cloudinary(account);
            _mediaSettings = mediaSettings.Value;
        }
      
        public async Task<ImageUploadResult> AddPhotoAsync(IFormFile file)
        {
            var uploadResult = new ImageUploadResult();
            if (file.Length > 0)
            {
                using var stream = file.OpenReadStream();
                var uploadParams = new ImageUploadParams
                {
                    File = new FileDescription(file.FileName, stream),
                    Transformation = new Transformation().Height(500).Width(500).Crop("fill").Gravity("face"),
                    Folder = "da-net8"
                };
                uploadResult = await _cloudinary.UploadAsync(uploadParams);
            }
            return uploadResult;
        }

        public async Task<DeletionResult> DeletePhotoAsync(string publicId)
        {
            var deleteParams = new DeletionParams(publicId);
            return await _cloudinary.DestroyAsync(deleteParams);
        }

        public async Task<VideoUploadResult> AddVideoAsync(IFormFile file)
        {
            var uploadResult = new VideoUploadResult();
            if (file.Length > 0 )
            {
                using var stream = file.OpenReadStream();
                var uploadParams = new VideoUploadParams
                {
                    File = new FileDescription(file.FileName, stream),
                    Folder = "da-net8/videos"
                };
                uploadResult = await _cloudinary.UploadAsync(uploadParams);
            }
            return uploadResult;
        }

        public async Task<DeletionResult> DeleteVideoAsync(string publicId)
        {
            var deleteParams = new DeletionParams(publicId);
            return await _cloudinary.DestroyAsync(deleteParams);
        }

        public async Task<RawUploadResult> AddAudioAsync(IFormFile file)
        {
            var uploadResult = new RawUploadResult();
            if (file.Length > 0 )
            {
                using var stream = file.OpenReadStream();
                var uploadParams = new RawUploadParams
                {
                    File = new FileDescription(file.FileName, stream),
                    Folder = "da-net8/audios"
                };
                uploadResult = await _cloudinary.UploadAsync(uploadParams);
            }
            return uploadResult;
        }

        public async Task<DeletionResult> DeleteAudioAsync(string publicId)
        {
            var deleteParams = new DeletionParams(publicId);
            return await _cloudinary.DestroyAsync(deleteParams);
        }
        
        public async Task<RawUploadResult> AddMediaAsync (IFormFile file , MediaType mediaType)
        {

                    switch (mediaType)
                    {
                        case MediaType.Image:
                            return await AddPhotoAsync(file);
                        case MediaType.Video:
                            return await AddVideoAsync(file);
                        case MediaType.Audio:
                            return await AddAudioAsync(file);
                        default:
                           throw new Exception("Unsupported media type.");
                   }            

        }
        public async Task<DeletionResult> DeleteMediaAsync(string publicId)
        {
            var deleteParams = new DeletionParams(publicId);
            return await _cloudinary.DestroyAsync(deleteParams);
           
        }

    }

}
