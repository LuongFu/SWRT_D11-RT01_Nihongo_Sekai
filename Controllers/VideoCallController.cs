﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JapaneseLearningPlatform.Controllers
{
    public class VideoCallController : Controller
    {
        // GET: VideoCallController
        public ActionResult Index()
        {
            return View();
        }

        // GET: VideoCallController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: VideoCallController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: VideoCallController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: VideoCallController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: VideoCallController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: VideoCallController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: VideoCallController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
