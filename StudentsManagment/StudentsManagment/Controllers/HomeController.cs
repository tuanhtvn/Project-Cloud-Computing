﻿using System.Diagnostics;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using StudentsManagment.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text;
using Newtonsoft.Json.Linq;
using System.Net.Http.Json;

namespace StudentsManagment.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    //string baseUrl = "http://JavaAPI:8090/";

    string baseUrl = "http://host.docker.internal:8090/";
    //string baseUrl = "http://springboot-docker-container:8090/";


    public HomeController(ILogger<HomeController> logger)
    {
        logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    public async Task<ActionResult> Students()
    {
        List<SinhVienModel> SVInfo = new List<SinhVienModel>();
        using (var client = new HttpClient())
        {
            //Passing service base url
            client.BaseAddress = new Uri(baseUrl);
            client.DefaultRequestHeaders.Clear();
            //Define request data format
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            //Sending request to find web api REST service resource GetAllEmployees using HttpClient
            HttpResponseMessage Res = await client.GetAsync("sinhviens");

            //Checking the response is successful or not which is sent using HttpClient
            if (Res.IsSuccessStatusCode)
            {
                //Storing the response details recieved from web api
                var SVResponse = Res.Content.ReadAsStringAsync().Result;

                //Deserializing the response recieved from web api and storing into the Employee list
                SVInfo = JsonConvert.DeserializeObject<List<SinhVienModel>>(SVResponse);
            }
            //returning the employee list to view

            return View(SVInfo);
        }
    }

    //private async SinhVienModel getStudentsApi(string msv)
    //{
    //    SinhVienModel foundsv = new SinhVienModel();

    //    List<SinhVienModel> SVInfo = new List<SinhVienModel>();
    //    using (var client = new HttpClient())
    //    {
    //        //Passing service base url
    //        client.BaseAddress = new Uri(baseUrl);
    //        client.DefaultRequestHeaders.Clear();
    //        //Define request data format
    //        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
    //        //Sending request to find web api REST service resource GetAllEmployees using HttpClient
    //        HttpResponseMessage Res = await client.GetAsync("sinhviens");

    //        //Checking the response is successful or not which is sent using HttpClient
    //        if (Res.IsSuccessStatusCode)
    //        {
    //            //Storing the response details recieved from web api
    //            var SVResponse = Res.Content.ReadAsStringAsync().Result;

    //            //Deserializing the response recieved from web api and storing into the Employee list
    //            SVInfo = JsonConvert.DeserializeObject<List<SinhVienModel>>(SVResponse);
    //        }
    //        //returning the employee list to view
    //    }

    //    foundsv = SVInfo.Find(x => x.masinhvien == msv);

    //    return foundsv;
    //}

    public IActionResult UpdateStudentForm(string mssv, string hodem, string ten, int namhoc, DateTime ngaysinh, int namnhaphoc, string gioitinh, string ctdt)
    {

        SinhVienModel sv = new SinhVienModel();
        sv.masinhvien = mssv;
        sv.hodem = hodem;
        sv.ten = ten;
        sv.namhoc = namhoc;
        sv.ngaysinh = ngaysinh;
        sv.namnhaphoc = namnhaphoc;
        sv.gioitinh = gioitinh == "Nam" ? true : false;
        sv.tenChuongTrinhDaoTao = ctdt;
        return View(sv);
    }

    [HttpPost]
    public ActionResult UpdateStudentForm(string mssv, string hoDem, string ten, int namHoc, int namNhapHoc, DateTime ngaysinh, string gioiTinh, string ctdt)
    {

        SinhVienModel student = new SinhVienModel();
        student.masinhvien = mssv;
        student.hodem = hoDem;
        student.ten = ten;
        student.namhoc = namHoc;
        student.namnhaphoc = namNhapHoc;
        student.ngaysinh = ngaysinh;
        student.gioitinh = gioiTinh == "Nam" ? true : false;
        student.tenChuongTrinhDaoTao = ctdt;

        using (var client = new HttpClient())
        {
            client.BaseAddress = new Uri(baseUrl + "sinhvien");

            //HTTP POST
            var postTask = client.PutAsJsonAsync<SinhVienModel>("sinhvien", student);
            postTask.Wait();

            var result = postTask.Result;
            if (result.IsSuccessStatusCode)
            {
                return RedirectToAction("Students");
            }
        }

        ModelState.AddModelError(string.Empty, "Server Error. Please contact administrator.");

        return View(student);
    }


    //[HttpPost]
    //public ActionResult DeleteStudent(string mssv)
    //{
    //    //JObject json = JObject.Parse(mssv);
    //    //var jsoon = @"{""id"":1,""name"":""Foo""}";
    //    using (var client = new HttpClient())
    //    {
    //        client.BaseAddress = new Uri(baseUrl+"sinhvien");
    //        //HTTP POST
    //        var postTask = client.DeleteFromJsonAsync<String>(baseUrl + "sinhvien")

    //        //Console.WriteLine("Tons "+mssv);
    //        postTask.Wait();

    //        var result = postTask.Result;
    //        if (result.IsSuccessStatusCode)
    //        {
    //            return RedirectToAction("Students");
    //        }
    //    }

    //    ModelState.AddModelError(string.Empty, "Server Error. Please contact administrator.");

    //    return RedirectToAction("Students");

    //}



    public IActionResult AddStudentForm()
    {
        return View();
    }

    [HttpPost]
    public ActionResult AddStudentForm(string mssv, string hoDem, string ten, int namHoc, int namNhapHoc, DateTime ngaysinh, string gioiTinh, string ctdt)
    {

        SinhVienModel student = new SinhVienModel();
        student.masinhvien = mssv;
        student.hodem = hoDem;
        student.ten = ten;
        student.namhoc = namHoc;
        student.namnhaphoc = namNhapHoc;
        student.ngaysinh = ngaysinh;
        student.gioitinh = gioiTinh == "Nam" ? true : false;
        student.tenChuongTrinhDaoTao = ctdt;

        using (var client = new HttpClient())
        {
            client.BaseAddress = new Uri(baseUrl + "sinhvien");

            //HTTP POST
            var postTask = client.PostAsJsonAsync<SinhVienModel>("sinhvien", student);
            postTask.Wait();

            var result = postTask.Result;
            if (result.IsSuccessStatusCode)
            {
                return RedirectToAction("Students");
            }
        }

        ModelState.AddModelError(string.Empty, "Server Error. Please contact administrator.");

        return View(student);
    }


    public async Task<ActionResult> ChuongTrinhDaotao()
    {
        List<ChuongTrinhDaoTaoModel> CtdtInfo = new List<ChuongTrinhDaoTaoModel>();
        using (var client = new HttpClient())
        {
            //Passing service base url
            client.BaseAddress = new Uri(baseUrl);
            client.DefaultRequestHeaders.Clear();
            //Define request data format
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            //Sending request to find web api REST service resource GetAllEmployees using HttpClient
            HttpResponseMessage Res = await client.GetAsync("chuongtrinhs");

            //Checking the response is successful or not which is sent using HttpClient
            if (Res.IsSuccessStatusCode)
            {
                //Storing the response details recieved from web api
                var ctdtResponse = Res.Content.ReadAsStringAsync().Result;

                //Deserializing the response recieved from web api and storing into the Employee list
                CtdtInfo = JsonConvert.DeserializeObject<List<ChuongTrinhDaoTaoModel>>(ctdtResponse);
            }
            //returning the employee list to view
   
            return View(CtdtInfo);
        }
    }


    public IActionResult ThemChuongTrinh()
    {
        return View();
    }

    [HttpPost]
    public ActionResult ThemChuongTrinh(string idChuongTrinh, string tenChuongTrinh, int SoTinhChi, int namBatDauHoc)
    {
        ChuongTrinhDaoTaoModel ChuongTrinh = new ChuongTrinhDaoTaoModel();
        ChuongTrinh.idChuongTrinhDaoTao = idChuongTrinh;
        ChuongTrinh.tenChuongTrinhDaoTao = tenChuongTrinh;
        ChuongTrinh.soTinChi = SoTinhChi;
        ChuongTrinh.namBatDauDaoTao = namBatDauHoc;

        using (var client = new HttpClient())
        {
            client.BaseAddress = new Uri(baseUrl + "chuongtrinh");

            //HTTP POST
            var postTask = client.PostAsJsonAsync<ChuongTrinhDaoTaoModel>("chuongtrinh", ChuongTrinh);
            postTask.Wait();

            var result = postTask.Result;
            if (result.IsSuccessStatusCode)
            {
                return RedirectToAction("ChuongTrinhDaoTao");
            }
        }

        ModelState.AddModelError(string.Empty, "Server Error. Please contact administrator.");

        return View(ChuongTrinh);
    }





    public IActionResult UpdateChuongTrinh(string idChuongTrinhDaoTao, string tenChuongTrinhDaoTao, float soTinChi, int namBatDauDaoTao)
    {

        ChuongTrinhDaoTaoModel ChuongTrinh = new ChuongTrinhDaoTaoModel();
        ChuongTrinh.idChuongTrinhDaoTao = idChuongTrinhDaoTao;
        ChuongTrinh.tenChuongTrinhDaoTao = tenChuongTrinhDaoTao;
        ChuongTrinh.soTinChi = soTinChi;
        ChuongTrinh.namBatDauDaoTao = namBatDauDaoTao;
        return View(ChuongTrinh);
    }

    [HttpPost]
    public ActionResult UpdateChuongTrinh(string tenChuongTrinhDaoTao, string idChuongTrinhDaoTao, int namBatDauDaoTao, float soTinChi)
    {


        ChuongTrinhDaoTaoModel ChuongTrinh = new ChuongTrinhDaoTaoModel();
        ChuongTrinh.idChuongTrinhDaoTao = idChuongTrinhDaoTao;
        ChuongTrinh.tenChuongTrinhDaoTao = tenChuongTrinhDaoTao;
        ChuongTrinh.soTinChi = soTinChi;
        ChuongTrinh.namBatDauDaoTao = namBatDauDaoTao;


        using (var client = new HttpClient())
        {
            client.BaseAddress = new Uri(baseUrl + "chuongtrinh");

            //HTTP POST
            var postTask = client.PutAsJsonAsync<ChuongTrinhDaoTaoModel>("chuongtrinh", ChuongTrinh);
            postTask.Wait();

            var result = postTask.Result;
            if (result.IsSuccessStatusCode)
            {
                return RedirectToAction("ChuongTrinhDaoTao");
            }
        }

        ModelState.AddModelError(string.Empty, "Server Error. Please contact administrator.");

        return View(ChuongTrinh);
    }


   
    public async Task<ActionResult> LopHocPhan()
    {
        List<LopHocPhanModel> LHPInfo = new List<LopHocPhanModel>();

        using (var client = new HttpClient())
        {
            //Passing service base url
            client.BaseAddress = new Uri(baseUrl);
            client.DefaultRequestHeaders.Clear();
            //Define request data format
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            //Sending request to find web api REST service resource GetAllEmployees using HttpClient
            HttpResponseMessage Res = await client.GetAsync("api/lophocphan");

            //Checking the response is successful or not which is sent using HttpClient
            if (Res.IsSuccessStatusCode)
            {
                //Storing the response details recieved from web api
                var LHPResponse = Res.Content.ReadAsStringAsync().Result;

                //Deserializing the response recieved from web api and storing into the Employee list
                LHPInfo = JsonConvert.DeserializeObject<List<LopHocPhanModel>>(LHPResponse);
            }
            //returning the employee list to view
            Console.WriteLine("tonss");

            return View(LHPInfo);
        }
    }
    public IActionResult ThemLopHocPhan()
    {
        return View();
    }



    

    //[HttpPost]
    //public ActionResult ThemLopHocPhan(int maLopHocPhan, string maMonHoc, int namHoc, string hocKy, int gioiHanSlg)
    //{
    //    LopHocPhanModel LHP = new LopHocPhanModel();
    //    LHP.maLopHocPhan = maLopHocPhan;
    //    LHP.maMonHoc = maMonHoc;
    //    LHP.namHoc = namHoc;
    //    LHP.hocKy = hocKy;
    //    LHP.gioiHanSlg = gioiHanSlg;

    //    using (var client = new HttpClient())
    //    {
    //        client.BaseAddress = new Uri(baseUrl + "api/");

    //        //HTTP POST
    //        var postTask = client.PostAsJsonAsync<LopHocPhanModel>("lophocphan", LHP);
    //        postTask.Wait();

    //        var result = postTask.Result;
    //        if (result.IsSuccessStatusCode)
    //        {
    //            return RedirectToAction("LopHocPhan");
    //        }
    //    }

    //    ModelState.AddModelError(string.Empty, "Server Error. Please contact administrator.");

    //    return View(LHP);
    //}





    //public IActionResult UpdateLopHocPhan(string idChuongTrinhDaoTao, string tenChuongTrinhDaoTao, float soTinChi, int namBatDauDaoTao)
    //{

    //    ChuongTrinhDaoTaoModel ChuongTrinh = new ChuongTrinhDaoTaoModel();
    //    ChuongTrinh.idChuongTrinhDaoTao = idChuongTrinhDaoTao;
    //    ChuongTrinh.tenChuongTrinhDaoTao = tenChuongTrinhDaoTao;
    //    ChuongTrinh.soTinChi = soTinChi;
    //    ChuongTrinh.namBatDauDaoTao = namBatDauDaoTao;
    //    return View(ChuongTrinh);
    //}

    //[HttpPost]
    //public ActionResult UpdateLopHocPhan(string tenChuongTrinhDaoTao, string idChuongTrinhDaoTao, int namBatDauDaoTao, float soTinChi)
    //{


    //    ChuongTrinhDaoTaoModel ChuongTrinh = new ChuongTrinhDaoTaoModel();
    //    ChuongTrinh.idChuongTrinhDaoTao = idChuongTrinhDaoTao;
    //    ChuongTrinh.tenChuongTrinhDaoTao = tenChuongTrinhDaoTao;
    //    ChuongTrinh.soTinChi = soTinChi;
    //    ChuongTrinh.namBatDauDaoTao = namBatDauDaoTao;


    //    using (var client = new HttpClient())
    //    {
    //        client.BaseAddress = new Uri(baseUrl + "chuongtrinh");

    //        //HTTP POST
    //        var postTask = client.PutAsJsonAsync<ChuongTrinhDaoTaoModel>("chuongtrinh", ChuongTrinh);
    //        postTask.Wait();

    //        var result = postTask.Result;
    //        if (result.IsSuccessStatusCode)
    //        {
    //            return RedirectToAction("ChuongTrinhDaoTao");
    //        }
    //    }

    //    ModelState.AddModelError(string.Empty, "Server Error. Please contact administrator.");

    //    return View(ChuongTrinh);
    //}









    public async Task<ActionResult> MonHoc()
    {
        List<MonHocModel> MonHocInfo = new List<MonHocModel>();
        using (var client = new HttpClient())
        {
            //Passing service base url
            client.BaseAddress = new Uri(baseUrl);
            client.DefaultRequestHeaders.Clear();
            //Define request data format
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            //Sending request to find web api REST service resource GetAllEmployees using HttpClient
            HttpResponseMessage Res = await client.GetAsync("api/monhoc");

            //Checking the response is successful or not which is sent using HttpClient
            if (Res.IsSuccessStatusCode)
            {
                //Storing the response details recieved from web api
                var MonHocResponse = Res.Content.ReadAsStringAsync().Result;

                //Deserializing the response recieved from web api and storing into the Employee list
                MonHocInfo = JsonConvert.DeserializeObject<List<MonHocModel>>(MonHocResponse);
            }
            //returning the employee list to view

            return View(MonHocInfo);
        }

    }
    public IActionResult ThemMonHoc()
    {
        return View();
    }

    [HttpPost]
    public ActionResult ThemMonHoc(string idMonHoc, string tenMonHoc, float soTinChi, string theloai, string idChuongTrinhDaoTao) 
    {
        MonHocModel mh = new MonHocModel();
        mh.maMonHoc = idMonHoc;
        mh.tenMonHoc = tenMonHoc;
        mh.soTinchi = soTinChi;
        mh.theLoai = theloai;
        mh.idChuongTrinhDaoTao = idChuongTrinhDaoTao;

       

        using (var client = new HttpClient())
        {
            client.BaseAddress = new Uri(baseUrl + "api/");

            //HTTP POST
            var postTask = client.PostAsJsonAsync<MonHocModel>("monhoc", mh);
            postTask.Wait();

            var result = postTask.Result;
            if (result.IsSuccessStatusCode)
            {
                return RedirectToAction("MonHoc");
            }
        }

        ModelState.AddModelError(string.Empty, "Server Error. Please contact administrator.");

        return View(mh);
    }





    public IActionResult UpdateMonHoc(string idMonHoc, string tenMonHoc, float soTinChi, string theloai, string idChuongTrinhDaoTao)
    {

        MonHocModel mh = new MonHocModel();
        mh.maMonHoc = idMonHoc;
        mh.tenMonHoc = tenMonHoc;
        mh.soTinchi = soTinChi;
        mh.theLoai = theloai;
        mh.idChuongTrinhDaoTao = idChuongTrinhDaoTao;
        return View(mh);
    }

    //[HttpPost]
    //public ActionResult UpdateMonHoc(string tenMonHoc, float soTinChi, string theloai, string idMonHoc, string idChuongTrinhDaoTao)
    //{


    //    MonHocModel mh = new MonHocModel();
    //    mh.maMonHoc = idMonHoc;
    //    mh.tenMonHoc = tenMonHoc;
    //    mh.soTinchi = soTinChi;
    //    mh.theLoai = theloai;
    //    mh.idChuongTrinhDaoTao = idChuongTrinhDaoTao;

    //    Console.WriteLine(mh.maMonHoc);
    //    Console.WriteLine(mh.tenMonHoc);
    //    Console.WriteLine(mh.soTinchi);
    //    Console.WriteLine(mh.theLoai);
    //    Console.WriteLine(mh.idChuongTrinhDaoTao);


    //    using (var client = new HttpClient())
    //    {
    //        client.BaseAddress = new Uri(baseUrl + "api/");

    //        //HTTP POST
    //        var postTask = client.PutAsJsonAsync<MonHocModel>("monhoc", mh);
    //        postTask.Wait();

    //        var result = postTask.Result;
    //        if (result.IsSuccessStatusCode)
    //        {
    //            return RedirectToAction("MonHoc");
    //        }
    //    }

    //    ModelState.AddModelError(string.Empty, "Server Error. Please contact administrator.");

    //    return View(mh);
    //}



    public async Task<ActionResult> ThamGiaHoc()
    {
        List<ThamGiaHocModel> ThamGiaHocInfo = new List<ThamGiaHocModel>();
        using (var client = new HttpClient())
        {
            //Passing service base url
            client.BaseAddress = new Uri(baseUrl);
            client.DefaultRequestHeaders.Clear();
            //Define request data format
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            //Sending request to find web api REST service resource GetAllEmployees using HttpClient
            HttpResponseMessage Res = await client.GetAsync("api/thamgiahoc");

            //Checking the response is successful or not which is sent using HttpClient
            if (Res.IsSuccessStatusCode)
            {
                //Storing the response details recieved from web api
                var ThamGiaHocResponse = Res.Content.ReadAsStringAsync().Result;

                //Deserializing the response recieved from web api and storing into the Employee list
                ThamGiaHocInfo = JsonConvert.DeserializeObject<List<ThamGiaHocModel>>(ThamGiaHocResponse);
            }
            //returning the employee list to view

            return View(ThamGiaHocInfo);
        }
    }



    public IActionResult ThemThamGia()
    {
        return View();
    }
    [HttpPost]
    public ActionResult ThemThamGia(string maLopHocPhan, string maSinhVien, float diemSo)
    {
        ThamGiaHocModel TGH = new ThamGiaHocModel();
        TGH.maLopHocPhan = maLopHocPhan;
        TGH.maSinhVien = maSinhVien;
        TGH.diemSo = diemSo;

        using (var client = new HttpClient())
        {
            client.BaseAddress = new Uri(baseUrl + "api/");

            //HTTP POST
            var postTask = client.PostAsJsonAsync<ThamGiaHocModel>("thamgiahoc", TGH);
            postTask.Wait();

            var result = postTask.Result;
            if (result.IsSuccessStatusCode)
            {
                return RedirectToAction("ThamGiaHoc");
            }
        }

        ModelState.AddModelError(string.Empty, "Server Error. Please contact administrator.");

        return View(TGH);
    }
    public IActionResult UpdateThamGia(string maLopHocPhan, string maSinhVien, float diemSo)
    {
        
        ThamGiaHocModel TGH = new ThamGiaHocModel();
        TGH.maLopHocPhan = maLopHocPhan;
        TGH.maSinhVien = maSinhVien;
        TGH.diemSo = diemSo;

        Console.WriteLine(TGH.maLopHocPhan);
        Console.WriteLine(TGH.maSinhVien);
        Console.WriteLine(TGH.diemSo);


        return View(TGH);
    }

    //[HttpPost]
    //public ActionResult UpdateChuongTrinh(string tenChuongTrinhDaoTao, string idChuongTrinhDaoTao, int namBatDauDaoTao, float soTinChi)
    //{


    //    ChuongTrinhDaoTaoModel ChuongTrinh = new ChuongTrinhDaoTaoModel();
    //    ChuongTrinh.idChuongTrinhDaoTao = idChuongTrinhDaoTao;
    //    ChuongTrinh.tenChuongTrinhDaoTao = tenChuongTrinhDaoTao;
    //    ChuongTrinh.soTinChi = soTinChi;
    //    ChuongTrinh.namBatDauDaoTao = namBatDauDaoTao;


    //    using (var client = new HttpClient())
    //    {
    //        client.BaseAddress = new Uri(baseUrl + "chuongtrinh");

    //        //HTTP POST
    //        var postTask = client.PutAsJsonAsync<ChuongTrinhDaoTaoModel>("chuongtrinh", ChuongTrinh);
    //        postTask.Wait();

    //        var result = postTask.Result;
    //        if (result.IsSuccessStatusCode)
    //        {
    //            return RedirectToAction("ChuongTrinhDaoTao");
    //        }
    //    }

    //    ModelState.AddModelError(string.Empty, "Server Error. Please contact administrator.");

    //    return View(ChuongTrinh);
    //}


    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}

