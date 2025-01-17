﻿using WorldRef.BusinessLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WorldRef.Models
{
    public class WorldRefSearchModel
    {
        /// <summary>
        /// Image Name
        /// </summary>
        public string ImageName { get; set; }

        /// <summary>
        /// Image Path
        /// </summary>
        public string ImagePath { get; set; }
        /// <summary>
        /// Image Path
        /// </summary>
        public bool? Link { get; set; }

        public int? id { get; set; }
        public string OrganizationName { get; set; }
        public string CustomerName { get; set; }
        public string ProjectName { get; set; }
        public string Type { get; set; }
        public string Year { get; set; }
        public string Country { get; set; }
        public string EmailId { get; set; }
        public bool IsOrganization { get; set; }
        public bool IsCustomer { get; set; }
        public bool IsProject { get; set; }
        public bool IsType { get; set; }
        public bool IsYear { get; set; }
        public bool IsCountry { get; set; }
        public bool IsEmail { get; set; }
        public Nullable<System.DateTime> Createdon { get; set; }
        public Nullable<int> userid { get; set; }
        public string ExcelPath { get; set; }
        public string ExcelName { get; set; }
        public string DisApproveReason { get; set; }
        public List<SelectListItem> RecruitersTypeList { get; set; }
        public string Name { get; set; }
        public string SignUpType { get; set; }
        public string ContactNumber { get; set; }
        public List<SelectListItem> TypeList { get; set; }
        public string Email { get; set; }
        public string Industry { get; set; }
        public string SignUpCountry { get; set; }
        public string ProfileFileName { get; set; }
        public string ProfilePath { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public int TotalLikeCount { get; set; }
        public List<SelectListItem> CountryList { get; set; }

        public List<ImageStructure> ListOfImage { get; set; }
        public List<SelectListItem> IndustryList { get; set; }
        public List<LinkedInUser> ListOfApprovedCredits { get; set; }
        public List<RegisterUserDAO> profilelist { get; set; }
        public string TotalLikes { get; set; }
        public string TotalDislikes { get; set; }
        public string CompanyProfile { get; set; }

        [Required(ErrorMessage = "Please Enter the rating")]
        [Range(typeof(int), "1", "10", ErrorMessage = "Enter Between 1 to 10")]
        public string Rating { get; set; }

        public string QualityRating { get; set; }
        public string UserRating { get; set; }
        public string CustomerIndustryType { get; set; }
        public string Status { get; set; }
        public string Description { get; set; }
        public string UserType { get; set; }
        public int ImageId { get; set; }
        public string CompanyLogo { get; set; }
        public List<WorldRefSearchModel> modeldataList { get; set; }
        public string ProfileUrl { get; set; }
        public string ProfileAttach { get; set; }
        public int countApprove { get; set; }
        public string countflg { get; set; }
        public string PhotoAttach { get; set; }
        public string Company { get; set; }
        public string UserFirstName { get; set; }
        public string IndustriesName { get; set; }
        public string CountryName { get; set; }


        public List<WorldRefSearchModel> AllData { get; set; }

        // public string FinancialRating { get; set; }
        public int? UserEdit_id { get; set; }

        public string ISO { get; set; }
        public string Iso_FileName { get; set; }
        public string Iso_ExpiryDate { get; set; }

        public string OHSOS { get; set; }
        public string Ohsos_FileName { get; set; }
        public string Ohsos_ExpiryDate { get; set; }

        [Required(ErrorMessage = "Please Enter the Employee")]
        public string EmployeeNo { get; set; }

        [Required(ErrorMessage = "Please Enter the Revenue")]
        public string Revenue { get; set; }

        public List<Rewards> _userreward = new List<Rewards>();

        public List<Certificates> _usercertificates = new List<Certificates>();

        public List<ProductCertificate_Edit> _productcertificate = new List<ProductCertificate_Edit>();


        public List<ManagementCertificate_Edit> _managementcertificateedit = new List<ManagementCertificate_Edit>();
        //public int ProductCertificateId { get; set; }
        //public IEnumerable<SelectListItem> productcertificate { get; set; }
        //public List<string> ProductTitle = new List<string>();


        [Required(ErrorMessage = "Please Enter the FinancialRating IssuedBy ")]
        public int _ratingid { get; set; }

        public SelectList GetFinancialRating
        {
            get
            {
                List<RatingIssuedBy> list = new List<RatingIssuedBy>()
                {
                    new RatingIssuedBy{Name="A. M. Best Company, USA ",Id=1},
                    new RatingIssuedBy{Name="Academic Credit Rating (ACR)[4], Spain",Id=2},
                    new RatingIssuedBy{Name="Agusto & Co.[5], Nigeria",Id=3},
                    new RatingIssuedBy{Name="Apoyo & Asociados Internacionales S.A.C.[6], Peru",Id=4},
                    new RatingIssuedBy{Name="ARC Ratings S.A. (former Companhia Portuguesa de Rating, S.A.), Portugal",Id=5},
                    new RatingIssuedBy{Name="ASSEKURATA Assekuranz Rating-Agentur GmbH [7], Germany",Id=6},
                    new RatingIssuedBy{Name="Atradius, Spain",Id=7},
                    new RatingIssuedBy{Name="Axesor SA[8], Spain",Id=8},
                    new RatingIssuedBy{Name="Bank Watch Ratings S.A.[9], Ecuador",Id=9},
                    new RatingIssuedBy{Name="Berkshire Hathaway Credit Ratings, Inc.[10], USA ",Id=10},
                    new RatingIssuedBy{Name="BRC Investor Services S.A.[11], Colombia",Id=11},
                    new RatingIssuedBy{Name="Bulgarian Credit Rating Agency AD[12], Bulgaria",Id=12},
                    new RatingIssuedBy{Name="Calificadora de Riesgo[13], Uruguay",Id=13},
                    new RatingIssuedBy{Name="Capital Intelligence (Cyprus) Ltd.[14], Cyprus ",Id=14},
                    new RatingIssuedBy{Name="Capital Standards Rating (CSR)[15], Kuwait ",Id=15},
                    new RatingIssuedBy{Name="Caribbean Information & Credit Rating Services, Ltd. (CariCRIS)[16], Trinidad and Tobago",Id=16},
                    new RatingIssuedBy{Name="CERVED Group S.p.A.[17], Italy ",Id=17},
                    new RatingIssuedBy{Name="Chengxin International Credit Rating Co., Ltd.[18],China",Id=18},
                    new RatingIssuedBy{Name="China Lianhe Credit Rating, Co. Ltd.[19],China",Id=19},
                    new RatingIssuedBy{Name="Clasificadora de Riesgo Humphreys, Ltda.[20], Chile ",Id=20},
                    new RatingIssuedBy{Name="Class y Asociados S.A.[21], Peru",Id=21},
                    new RatingIssuedBy{Name="Compagnie Française d-Assurance pour le Commerce Extérieur (COFACE), France",Id=22},
                    new RatingIssuedBy{Name="Credit Analysis & Research, Ltd. (CARE)[22], India ",Id=23},
                    new RatingIssuedBy{Name="Credit Rating Agency of Bangladesh, Ltd. (CRAB)[23], Bangladesh",Id=24},
                    new RatingIssuedBy{Name="Credit Rating Information and Services Ltd., Bangladesh",Id=25},
                    new RatingIssuedBy{Name="Credit-Rating[24], Ukraine",Id=26},
                    new RatingIssuedBy{Name="Creditreform Rating AG[25], Germany",Id=27},
                    new RatingIssuedBy{Name="CRIF S.p.A.[26], Italy ",Id=28},
                    new RatingIssuedBy{Name="CRISIL, Ltd.[27], India ",Id=29},
                    new RatingIssuedBy{Name="Dagong Europe Credit Rating, S.r.l. (Dagong Europe)[28], Italy ",Id=30},
                    new RatingIssuedBy{Name="Dagong Global Credit Rating,China",Id=31},
                    new RatingIssuedBy{Name="Demotech, Inc.[29], USA ",Id=32},
                    new RatingIssuedBy{Name="Dominion Bond Rating Service Ltd., Canada ",Id=33},
                    new RatingIssuedBy{Name="Dun & Bradstreet, USA ",Id=34},
                    new RatingIssuedBy{Name="Egan-Jones Rating Company, USA ",Id=35},
                    new RatingIssuedBy{Name="Emerging Credit Rating Ltd (ECRL)[30], Bangladesh",Id=36},
                    new RatingIssuedBy{Name="Equilibrium Clasificadora de Riesgo[31], Peru",Id=37},
                    new RatingIssuedBy{Name="Euler Hermes Rating GmbH, Germany",Id=38},
                    new RatingIssuedBy{Name="Euromoney Country Risk (ECR), USA ",Id=39},
                    new RatingIssuedBy{Name="European Rating Agency, A.S. (ERA) [32], Slovakia",Id=40},
                    new RatingIssuedBy{Name="EuroRating, Sp.z.o.o.[33], Poland ",Id=41},
                    new RatingIssuedBy{Name="Expert RA[34], Russia ",Id=42},
                    new RatingIssuedBy{Name="Fedafin[35], Switzerland",Id=43},
                    new RatingIssuedBy{Name="Feller Rate Clasificadora de Riesgo[36], Chile ",Id=44},
                    new RatingIssuedBy{Name="Feri EuroRating Services AG[37], Germany",Id=45},
                    new RatingIssuedBy{Name="Fitch Ratings, USA ",Id=46},
                    new RatingIssuedBy{Name="Fitch Ratings Polska[38], Poland ",Id=47},
                    new RatingIssuedBy{Name="GBB-Rating Gesellschaft für Bonitätsbeurteilung mbH[39], Germany",Id=48},
                    new RatingIssuedBy{Name="Global Credit Ratings Co.[40], South Africa",Id=49},
                    new RatingIssuedBy{Name="HR Ratings, S.A. de C.V.[41], Mexico ",Id=50},
                    new RatingIssuedBy{Name="ICAP Group, S.A.[42], Greece ",Id=51},
                    new RatingIssuedBy{Name="Interfax Rating Agency (IRA)[43], Russia ",Id=52},
                    new RatingIssuedBy{Name="International Non-Profit Credit Rating Agency (INCRA), USA ",Id=53},
                    new RatingIssuedBy{Name="Investment Information and Credit Rating Agency (ICRA), India ",Id=54},
                    new RatingIssuedBy{Name="Islamic International Rating Agency, B.S.C. (IIRA)[44], Bahrain",Id=55},
                    new RatingIssuedBy{Name="Japan Credit Rating Agency, Ltd., Japan ",Id=56},
                    new RatingIssuedBy{Name="JCR-VIS Credit Rating Co. Ltd.[45], Pakistan",Id=57},
                    new RatingIssuedBy{Name="Kobirate A.Ş.[46], Turkey ",Id=58},
                    new RatingIssuedBy{Name="Korea Investors Service, Inc. (KIS)[47], South Korea",Id=59},
                    new RatingIssuedBy{Name="Korea Ratings Corporation[48], South Korea",Id=60},
                    new RatingIssuedBy{Name="Kroll Bond Rating Agency[49], USA ",Id=61},
                    new RatingIssuedBy{Name="Levin and Goldstein[50], Zambia ",Id=62},
                    new RatingIssuedBy{Name="Malaysian Rating Corporation Berhad (MARC)[51], Malaysia",Id=63},
                    new RatingIssuedBy{Name="Moody-s Investors Service, USA ",Id=64},
                    new RatingIssuedBy{Name="Morningstar, Inc., USA ",Id=65},
                    new RatingIssuedBy{Name="Muros Ratings[52], Russia ",Id=66},
                    new RatingIssuedBy{Name="National Information & Credit Evaluation, Inc. (NICE)[53], South Korea",Id=67},
                    new RatingIssuedBy{Name="NUS Risk Management Institute[54], Singapore",Id=68},
                    new RatingIssuedBy{Name="ONICRA Credit Rating Agency of India, Ltd.[55], India ",Id=69},
                    new RatingIssuedBy{Name="Ontonix[56], Italy ",Id=70},
                    new RatingIssuedBy{Name="Pacific Credit Rating (PCR)[57], Peru",Id=71},
                    new RatingIssuedBy{Name="Pakistan Credit Rating Agency, Ltd. (PACRA)[58], Pakistan",Id=72},
                    new RatingIssuedBy{Name="Pefindo Credit Rating Agency[59], Indonesia",Id=73},
                    new RatingIssuedBy{Name="Philippine Rating Services, Corp. (PhilRatings)[60], Philippines",Id=74},
                    new RatingIssuedBy{Name="Public Sector Credit Solutions, USA ",Id=75},
                    new RatingIssuedBy{Name="RAM Rating Services Berhad[61], Malaysia",Id=76},
                    new RatingIssuedBy{Name="Rapid Ratings International, Inc. (RRI), USA ",Id=77},
                    new RatingIssuedBy{Name="Rating and Investment Information, Inc. (R&I)[62], Japan ",Id=78},
                    new RatingIssuedBy{Name="RusRating, Russia ",Id=79},
                    new RatingIssuedBy{Name="SAHA A.Ş.[63], Turkey ",Id=80},
                    new RatingIssuedBy{Name="Scope Credit Rating GmbH[64], Germany",Id=81},
                    new RatingIssuedBy{Name="Seoul Credit Rating & Information, Inc. (SCI)[65], South Korea",Id=82},
                    new RatingIssuedBy{Name="Shanghai Credit Information Services Co., Ltd. (CIS)[66],China",Id=83},
                    new RatingIssuedBy{Name="SMERA Ratings, Ltd.[67], India ",Id=84},
                    new RatingIssuedBy{Name="Sociedad Calificadora de Riesgo Centroamericana, S.A. (SCRiesgo)[68], Costa Rica",Id=85},
                    new RatingIssuedBy{Name="Spread Research, France ",Id=86},
                    new RatingIssuedBy{Name="SR Rating, Ltda.[69], Brazil ",Id=87},
                    new RatingIssuedBy{Name="Standard & Poor-s, USA ",Id=88},
                    new RatingIssuedBy{Name="Taiwan Ratings, Corp. (TCR)[70],China",Id=89},
                    new RatingIssuedBy{Name="Thai Rating and Information Services Co., Ltd. (TRIS)[71], Thailand",Id=90},
                    new RatingIssuedBy{Name="The Economist Intelligence Unit, Ltd., USA ",Id=91},
                    new RatingIssuedBy{Name="Turkish Credit Rating A.Ş.[72], Turkey ",Id=92},
                    new RatingIssuedBy{Name="TURKrating[73], Turkey ",Id=93},
                    new RatingIssuedBy{Name="Veda, Australia",Id=94},
                    new RatingIssuedBy{Name="Veribanc, Inc.[74], USA ",Id=95},
                    new RatingIssuedBy{Name="Weiss Ratings, LLC[75], USA ",Id=96},
                    new RatingIssuedBy{Name="Wikirating, Switzerland",Id=97},
                };

                return new SelectList(list, "Id", "Name");
            }
        }

    }


    public class ProductCertificate_Edit
    {
        public int Id { get; set; }
        public string CertificateName { get; set; }
        public string CertificateIssuedBy { get; set; }
        public string CertificateAttactedFile { get; set; }
        public string Startdate { get; set; }
        public string Enddate { get; set; }
        public int? CertificateId { get; set; }
        public int? IssuedId { get; set; }
    }

    public class ManagementCertificate_Edit
    {
        public int Id { get; set; }
        public string CertificateName { get; set; }
        public string CertificateIssuedBy { get; set; }
        public string CertificateAttactedFile { get; set; }
        public string Startdate { get; set; }
        public string Enddate { get; set; }
        public int? CertificateId { get; set; }
        public int? IssuedId { get; set; }
    }

    public class ManagementCertificate
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Name { get; set; }
    }

    public class ProductCertificate
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Name { get; set; }
    }
    public class IssuedCertificate
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
    }

    public class Rewards
    {
        public int Id { get; set; }
        public string RewardName { get; set; }
        public string RewardFileName { get; set; }
    }


    public class Certificates
    {
        public int Id { get; set; }
        public string CertificateName { get; set; }
        public string CertificateFileName { get; set; }
    }

    public class RatingIssuedBy
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}