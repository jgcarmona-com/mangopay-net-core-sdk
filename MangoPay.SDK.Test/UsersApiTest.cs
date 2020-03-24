using MangoPay.SDK.APIs.Auth;
using MangoPay.SDK.APIs.Users;
using MangoPay.SDK.Core;
using MangoPay.SDK.Core.Enumerations;
using MangoPay.SDK.Core.Exceptions;
using MangoPay.SDK.Core.Filters;
using MangoPay.SDK.Entities;
using MangoPay.SDK.Entities.GET;
using MangoPay.SDK.Entities.POST;
using MangoPay.SDK.Entities.PUT;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Xunit;

namespace MangoPay.SDK.Test
{
    public class UsersApiTest : TestBase
    {
        private UserApi _objectToTest;
        private Mock<ILogger<UserApi>> _loggerMock;
        public UsersApiTest()
        {
            // Arrange: 
            var services = new ServiceCollection();
            var configuration = TestHelper.GetIConfigurationRoot();
            // Act:
            services.AddMangoPayServices(configuration);
            var provider = services.BuildServiceProvider();
            _objectToTest = provider.GetRequiredService<IUsersApi>() as UserApi;
        }

        [Fact]
        public void UserServiceContructor_ShouldReturnTheObject()
        {
            // Arrange:
            _loggerMock = new Mock<ILogger<UserApi>>();
            var authServiceMock = new Mock<IAuthApi>();
            var config = GetConfiguration();
            // Act:
            _objectToTest = new UserApi(config, _loggerMock.Object, authServiceMock.Object);
            // Assert:
            Assert.NotNull(_objectToTest);
        }

        [Fact]
        public void WhenCreatingValidNaturalUser_ItShouldReturnNaturalUserDto()
        {
            // Arrange:
            var user = new UserNaturalPostDTO(
                "john.doe@sample2.org",
                "John",
                "Doe",
                new DateTime(1975, 12, 21, 0, 0, 0),
                CountryIso.FR,
                CountryIso.FR)
            {
                Occupation = "programmer",
                IncomeRange = 3,
                Address = new Address
                {
                    AddressLine1 = "Address line 1",
                    AddressLine2 = "Address line 2",
                    City = "City",
                    Country = CountryIso.PL,
                    PostalCode = "11222",
                    Region = "Region"
                },
                Capacity = CapacityType.DECLARATIVE
            };

            // Act:
            var result = _objectToTest.Create(user).Result;

            // Assert:
            Assert.NotNull(result);
            Assert.IsType<UserNaturalDTO>(result);
            Assert.True(!string.IsNullOrEmpty(result.Id));
        }

        [Fact]
        public void WhenCreatingValidLegalUser_ItShouldReturnLegalUserDto()
        {
            // Arrange:
            UserLegalPostDTO user = new UserLegalPostDTO(
             "john.doe@sample.org",
             "MartixSampleOrg",
             "345",
             LegalPersonType.BUSINESS,
             "JohnUbo",
             "DoeUbo",
             new DateTime(1975, 12, 21, 0, 0, 0),
             CountryIso.PL,
             CountryIso.PL)
            {
                HeadquartersAddress = new Address
                {
                    AddressLine1 = "Address line ubo 1",
                    AddressLine2 = "Address line ubo 2",
                    City = "CityUbo",
                    Country = CountryIso.PL,
                    PostalCode = "11222",
                    Region = "RegionUbo"
                },

                LegalRepresentativeAddress = new Address
                {
                    AddressLine1 = "Address line ubo 1",
                    AddressLine2 = "Address line ubo 2",
                    City = "CityUbo",
                    Country = CountryIso.PL,
                    PostalCode = "11222",
                    Region = "RegionUbo"
                },

                LegalRepresentativeEmail = "john.doe@sample.org",
                LegalRepresentativeBirthday = new DateTime(1975, 12, 21, 0, 0, 0),
                Email = "john.doe@sample.org"
            };

            // Act:
            var result = _objectToTest.Create(user).Result;

            // Assert:
            Assert.NotNull(result);
            Assert.IsType<UserLegalDTO>(result);
            Assert.True(!string.IsNullOrEmpty(result.Id));
        }

        [Fact]
        public void WhenGettingAllUsers_ItShouldReturnAPaginatedListOfUSers()
        {
            // Arrange: 
            // Act:
            var result = _objectToTest.GetAll().Result;

            // Assert:
            Assert.NotNull(result);
            Assert.IsType<ListPaginated<UserDTO>>(result);
        }

        [Fact]
        public async Task WhenGettingWalletsFromUnknownUser_ItShouldThrowResponseException()
        {
            // Arrange: 
            // Act:
            ResponseException ex = await Assert.ThrowsAsync<ResponseException>(() => _objectToTest.GetWallets("12345"));
            // Assert:
            Assert.Equal(404, ex.ResponseStatusCode);
            Assert.NotNull(ex.Message);
            Assert.NotNull(ex.ResponseErrorRaw);
            Assert.NotNull(ex.ResponseError);
            Assert.NotEmpty(ex.ResponseError.errors);
            Assert.True(ex.ResponseError.errors.ContainsKey("RessourceNotFound"));
        }
        [Fact]
        public void Test_Users_CreateNatural()
        {
            try
            {
                UserNaturalDTO john = TestHelper.GetJohn();
                Assert.True(john.Id.Length > 0);
                Assert.True(john.PersonType == PersonType.NATURAL);
            }
            catch (Exception ex)
            {
                Assert.True(false, ex.Message);
            }
        }

        [Fact]
        public void Test_Users_CreateLegal()
        {
            try
            {
                UserLegalDTO matrix = TestHelper.GetMatrix();
                Assert.True(matrix.Id.Length > 0);
                Assert.True(matrix.PersonType == PersonType.LEGAL);
            }
            catch (Exception ex)
            {
                Assert.True(false, ex.Message);
            }
        }

        [Fact]
        public void Test_Users_CreateLegal_PassesIfRequiredPropsProvided()
        {
            try
            {
                UserLegalPostDTO userPost = new UserLegalPostDTO("email@email.org", "SomeOtherSampleOrg","77272", LegalPersonType.BUSINESS, "RepFName", "RepLName", new DateTime(1975, 12, 21, 0, 0, 0), CountryIso.FR, CountryIso.FR);

                UserLegalDTO userCreated = _objectToTest.Create(userPost).Result;

                UserLegalDTO userGet = _objectToTest.GetLegal(userCreated.Id).Result;

                Assert.True(userCreated.Id.Length > 0, "Created successfully after required props set.");

                TestHelper.AssertEqualInputProps(userCreated, userGet);
            }
            catch (Exception ex)
            {
                Assert.True(false, ex.Message);
            }
        }

        [Fact]
        public void Test_Users_GetNatural()
        {
            try
            {
                UserNaturalDTO john = TestHelper.GetNewJohn();

                UserNaturalDTO userNatural = _objectToTest.GetNatural(john.Id).Result;

                Assert.True(userNatural.PersonType == PersonType.NATURAL);
                Assert.True(userNatural.Id == john.Id);

                TestHelper.AssertEqualInputProps(userNatural, john);
            }
            catch (Exception ex)
            {
                Assert.True(false, ex.Message);
            }
        }

        [Fact]
        public async Task Test_Users_GetNatural_FailsForLegalUser()
        {
            UserLegalDTO matrix = null;

            matrix = TestHelper.GetMatrix();

            ResponseException ex = await Assert.ThrowsAsync<ResponseException>(() => _objectToTest.GetNatural(matrix.Id));
            // Assert:
            Assert.Equal(404, ex.ResponseStatusCode);
            Assert.NotNull(ex.Message);
            Assert.NotNull(ex.ResponseErrorRaw);
            Assert.NotNull(ex.ResponseError);
            Assert.NotEmpty(ex.ResponseError.errors);
            Assert.True(ex.ResponseError.errors.ContainsKey("RessourceNotFound"));

        }

        [Fact]
        public async Task Test_Users_GetLegal_FailsForNaturalUser()
        {
            UserNaturalDTO john = null;

            john = TestHelper.GetJohn();

            ResponseException ex = await Assert.ThrowsAsync<ResponseException>(() => _objectToTest.GetLegal(john.Id));
            // Assert:
            Assert.Equal(404, ex.ResponseStatusCode);
            Assert.NotNull(ex.Message);
            Assert.NotNull(ex.ResponseErrorRaw);
            Assert.NotNull(ex.ResponseError);
            Assert.NotEmpty(ex.ResponseError.errors);
            Assert.True(ex.ResponseError.errors.ContainsKey("RessourceNotFound"));
        }

        [Fact]
        public void Test_Users_GetLegal()
        {
            try
            {
                UserLegalDTO matrix = TestHelper.GetMatrix();

                UserDTO userLegal = _objectToTest.GetLegal(matrix.Id).Result;

                TestHelper.AssertEqualInputProps((UserLegalDTO)userLegal, matrix);
            }
            catch (Exception ex)
            {
                Assert.True(false, ex.Message);
            }
        }

        [Fact]
        public void Test_Users_GetAll()
        {
            try
            {
                ListPaginated<UserDTO> users = _objectToTest.GetAll().Result;

                Assert.NotNull(users);
                Assert.True(users.Count > 0);


                // test sorting
                ListPaginated<UserDTO> result = null;
                ListPaginated<UserDTO> result2 = null;

                Pagination pagination = new Pagination(1, 2);
                Sort sort = new Sort();
                sort.AddField("CreationDate", SortDirection.asc);
                result = _objectToTest.GetAll(pagination, sort).Result;
                Assert.NotNull(result);
                Assert.True(result.Count > 0);

                sort = new Sort();
                sort.AddField("CreationDate", SortDirection.desc);
                result2 = _objectToTest.GetAll(pagination, sort).Result;
                Assert.NotNull(result2);
                Assert.True(result2.Count > 0);

                Assert.True(result[0].Id != result2[0].Id);
            }
            catch (Exception ex)
            {
                Assert.True(false, ex.Message);
            }
        }

        [Fact]
        public void Test_Users_Save_Natural()
        {
            try
            {
                UserNaturalDTO john = TestHelper.GetJohn();

                UserNaturalPutDTO johnPut = new UserNaturalPutDTO
                {
                    LastName = john.LastName + " - CHANGED",
                    Nationality = CountryIso.DK
                };

                UserNaturalDTO userSaved = _objectToTest.UpdateNatural(johnPut, john.Id).Result;
                UserNaturalDTO userFetched = _objectToTest.GetNatural(john.Id).Result;

                Assert.Equal(johnPut.LastName, userSaved.LastName);
                TestHelper.AssertEqualInputProps(userSaved, userFetched);
            }
            catch (Exception ex)
            {
                Assert.True(false, ex.Message);
            }
        }

        [Fact]
        public void Test_Users_Save_Natural_NonASCII()
        {
            try
            {
                UserNaturalDTO john = TestHelper.GetJohn();

                UserNaturalPutDTO johnPut = new UserNaturalPutDTO
                {
                    Tag = john.Tag,
                    Email = john.Email,
                    FirstName = john.FirstName,
                    LastName = john.LastName + " - CHANGED (éèęóąśłżźćń)",
                    Address = john.Address,
                    Birthday = john.Birthday,
                    Nationality = john.Nationality,
                    CountryOfResidence = john.CountryOfResidence,
                    Occupation = john.Occupation,
                    IncomeRange = john.IncomeRange
                };

                UserNaturalDTO userSaved = _objectToTest.UpdateNatural(johnPut, john.Id).Result;
                UserNaturalDTO userFetched = _objectToTest.GetNatural(john.Id).Result;

                Assert.Equal(johnPut.LastName, userSaved.LastName);
                TestHelper.AssertEqualInputProps(userSaved, userFetched);
            }
            catch (Exception ex)
            {
                Assert.True(false, ex.Message);
            }
        }

        [Fact]
        public void Test_Users_Save_Legal()
        {
            try
            {
                UserLegalDTO matrix = TestHelper.GetMatrix();

                UserLegalPutDTO matrixPut = new UserLegalPutDTO
                {
                    Tag = matrix.Tag,
                    Email = matrix.Email,
                    Name = matrix.Name,
                    LegalPersonType = matrix.LegalPersonType,
                    HeadquartersAddress = matrix.HeadquartersAddress,
                    LegalRepresentativeFirstName = matrix.LegalRepresentativeFirstName,
                    LegalRepresentativeLastName = matrix.LegalRepresentativeLastName + " - CHANGED",
                    LegalRepresentativeAddress = matrix.LegalRepresentativeAddress,
                    LegalRepresentativeEmail = matrix.LegalRepresentativeEmail,
                    LegalRepresentativeBirthday = matrix.LegalRepresentativeBirthday,
                    LegalRepresentativeNationality = matrix.LegalRepresentativeNationality,
                    LegalRepresentativeCountryOfResidence = matrix.LegalRepresentativeCountryOfResidence
                };

                UserLegalDTO userSaved = _objectToTest.UpdateLegal(matrixPut, matrix.Id).Result;
                UserLegalDTO userFetched = _objectToTest.GetLegal(userSaved.Id).Result;

                Assert.Equal(matrixPut.LegalRepresentativeLastName, userFetched.LegalRepresentativeLastName);
                TestHelper.AssertEqualInputProps(userSaved, userFetched);
            }
            catch (Exception ex)
            {
                Assert.True(false, ex.Message);
            }
        }

        [Fact]
        public void Test_Users_CreateBankAccount_IBAN()
        {
            try
            {
                UserNaturalDTO john = TestHelper.GetJohn();
                BankAccountDTO account = TestHelper.GetJohnsAccount();

                Assert.True(account.Id.Length > 0);
                Assert.Equal(account.UserId, john.Id);
            }
            catch (Exception ex)
            {
                Assert.True(false, ex.Message);
            }
        }

        [Fact]
        public void Test_Users_CreateBankAccount_GB()
        {
            try
            {
                UserNaturalDTO john = TestHelper.GetJohn();
                BankAccountGbPostDTO account = new BankAccountGbPostDTO(john.FirstName + " " + john.LastName, john.Address, "63956474");
                account.SortCode = "200000";

                BankAccountDTO createAccount = _objectToTest.CreateBankAccountGb(john.Id, account).Result;

                Assert.True(createAccount.Id.Length > 0);
                Assert.True(createAccount.UserId == (john.Id));
                Assert.True(createAccount.Type == BankAccountType.GB);
                Assert.True(((BankAccountGbDTO)createAccount).AccountNumber == "63956474");
                Assert.True(((BankAccountGbDTO)createAccount).SortCode == "200000");
            }
            catch (Exception ex)
            {
                Assert.True(false, ex.Message);
            }
        }

        [Fact]
        public void Test_Users_CreateBankAccount_US()
        {
            try
            {
                UserNaturalDTO john = TestHelper.GetJohn();
                BankAccountUsPostDTO account = new BankAccountUsPostDTO(john.FirstName + " " + john.LastName, john.Address, "234234234234", "234334789");

                BankAccountDTO createAccount = _objectToTest.CreateBankAccountUs(john.Id, account).Result;

                Assert.True(createAccount.Id.Length > 0);
                Assert.True(createAccount.UserId == (john.Id));
                Assert.True(createAccount.Type == BankAccountType.US);
                Assert.True(((BankAccountUsDTO)createAccount).AccountNumber == "234234234234");
                Assert.True(((BankAccountUsDTO)createAccount).ABA == "234334789");
                Assert.True(((BankAccountUsDTO)createAccount).DepositAccountType == DepositAccountType.CHECKING);

                account.DepositAccountType = DepositAccountType.SAVINGS;
                BankAccountDTO createAccountSavings = _objectToTest.CreateBankAccountUs(john.Id, account).Result;

                Assert.True(createAccountSavings.Id.Length > 0);
                Assert.True(createAccountSavings.UserId == (john.Id));
                Assert.True(createAccountSavings.Type == BankAccountType.US);
                Assert.True(((BankAccountUsDTO)createAccountSavings).AccountNumber == "234234234234");
                Assert.True(((BankAccountUsDTO)createAccountSavings).ABA == "234334789");
                Assert.True(((BankAccountUsDTO)createAccountSavings).DepositAccountType == DepositAccountType.SAVINGS);
            }
            catch (Exception ex)
            {
                Assert.True(false, ex.Message);
            }
        }

        [Fact]
        public void Test_Users_CreateBankAccount_CA()
        {
            try
            {
                UserNaturalDTO john = TestHelper.GetJohn();
                BankAccountCaPostDTO account = new BankAccountCaPostDTO(john.FirstName + " " + john.LastName, john.Address, "TestBankName", "123", "12345", "234234234234");

                BankAccountDTO createAccount = _objectToTest.CreateBankAccountCa(john.Id, account).Result;

                Assert.True(createAccount.Id.Length > 0);
                Assert.True(createAccount.UserId == (john.Id));
                Assert.True(createAccount.Type == BankAccountType.CA);
                Assert.True(((BankAccountCaDTO)createAccount).AccountNumber == "234234234234");
                Assert.True(((BankAccountCaDTO)createAccount).BankName == "TestBankName");
                Assert.True(((BankAccountCaDTO)createAccount).BranchCode == "12345");
                Assert.True(((BankAccountCaDTO)createAccount).InstitutionNumber == "123");
            }
            catch (Exception ex)
            {
                Assert.True(false, ex.Message);
            }
        }

        [Fact]
        public void Test_Users_CreateBankAccount_OTHER()
        {
            try
            {
                UserNaturalDTO john = TestHelper.GetJohn();
                BankAccountOtherPostDTO account = new BankAccountOtherPostDTO(john.FirstName + " " + john.LastName, john.Address, "234234234234", "BINAADADXXX");
                account.Type = BankAccountType.OTHER;
                account.Country = CountryIso.FR;

                BankAccountDTO createAccount = _objectToTest.CreateBankAccountOther(john.Id, account).Result;

                Assert.True(createAccount.Id.Length > 0);
                Assert.True(createAccount.UserId == (john.Id));
                Assert.True(createAccount.Type == BankAccountType.OTHER);
                Assert.True(((BankAccountOtherDTO)createAccount).Type == BankAccountType.OTHER);
                Assert.True(((BankAccountOtherDTO)createAccount).Country == CountryIso.FR);
                Assert.True(((BankAccountOtherDTO)createAccount).AccountNumber == ("234234234234"));
                Assert.True(((BankAccountOtherDTO)createAccount).BIC == ("BINAADADXXX"));
            }
            catch (Exception ex)
            {
                Assert.True(false, ex.Message);
            }
        }

        [Fact]
        public void Test_Users_CreateBankAccount()
        {
            try
            {
                UserNaturalDTO john = TestHelper.GetJohn();
                BankAccountIbanDTO account = TestHelper.GetJohnsAccount();

                Assert.True(account.Id.Length > 0);
                Assert.True(account.UserId == (john.Id));
            }
            catch (Exception ex)
            {
                Assert.True(false, ex.Message);
            }
        }

        [Fact]
        public void Test_Users_BankAccount()
        {
            try
            {
                UserNaturalDTO john = TestHelper.GetJohn();
                BankAccountIbanDTO account = TestHelper.GetJohnsAccount();

                BankAccountIbanDTO accountFetched = _objectToTest.GetBankAccountIban(john.Id, account.Id).Result;

                TestHelper.AssertEqualInputProps(account, accountFetched);
            }
            catch (Exception ex)
            {
                Assert.True(false, ex.Message);
            }
        }

        [Fact]
        public void Test_Users_BankAccounts()
        {
            try
            {
                UserNaturalDTO john = TestHelper.GetJohn();
                BankAccountIbanDTO account = TestHelper.GetJohnsAccount();
                Pagination pagination = new Pagination(1, 12);

                ListPaginated<BankAccountDTO> list = _objectToTest.GetBankAccounts(john.Id, pagination).Result;

                int listIndex;
                for (listIndex = 0; listIndex < list.Count; listIndex++)
                {
                    if (list[listIndex].Id == account.Id) break;
                }

                Assert.True(list[listIndex] is BankAccountDTO);

                BankAccountIbanDTO castedBankAccount = _objectToTest.GetBankAccountIban(john.Id, list[listIndex].Id).Result;

                Assert.True(account.Id == castedBankAccount.Id);
                TestHelper.AssertEqualInputProps(account, castedBankAccount);
                Assert.True(pagination.Page == 1);
                Assert.True(pagination.ItemsPerPage == 12);


                // test sorting
                ListPaginated<BankAccountDTO> result = null;
                ListPaginated<BankAccountDTO> result2 = null;

                BankAccountOtherPostDTO account2 = new BankAccountOtherPostDTO(john.FirstName + " " + john.LastName, john.Address, "234234234234", "BINAADADXXX");
                account2.Type = BankAccountType.OTHER;
                account2.Country = CountryIso.FR;

                var createdBankAccount = _objectToTest.CreateBankAccountOther(john.Id, account2).Result;

                pagination = new Pagination(1, 2);
                Sort sort = new Sort();
                sort.AddField("CreationDate", SortDirection.asc);
                result = _objectToTest.GetBankAccounts(john.Id, pagination, sort).Result;
                Assert.NotNull(result);
                Assert.True(result.Count > 0);

                sort = new Sort();
                sort.AddField("CreationDate", SortDirection.desc);
                result2 = _objectToTest.GetBankAccounts(john.Id, pagination, sort).Result;
                Assert.NotNull(result2);
                Assert.True(result2.Count > 0);

                Assert.True(result[0].Id != result2[0].Id);
            }
            catch (Exception ex)
            {
                Assert.True(false, ex.Message);
            }
        }

        [Fact]
        public void Test_Users_UpdateBankAccount()
        {
            try
            {
                UserNaturalDTO john = TestHelper.GetJohn();
                BankAccountIbanDTO account = TestHelper.GetJohnsAccount();

                Assert.True(account.Id.Length > 0);
                Assert.True(account.UserId == (john.Id));
                Assert.True(account.Active);

                // disactivate bank account
                DisactivateBankAccountPutDTO disactivateBankAccount = new DisactivateBankAccountPutDTO();
                disactivateBankAccount.Active = false;

                BankAccountDTO result = _objectToTest.UpdateBankAccount(john.Id, disactivateBankAccount, account.Id).Result;

                Assert.NotNull(result);
                Assert.True(account.Id == result.Id);
                Assert.False(result.Active);
            }
            catch (Exception ex)
            {
                Assert.True(false, ex.Message);
            }
        }

        [Fact]
        public void Test_Users_CreateKycDocument()
        {
            try
            {
                KycDocumentDTO kycDocument = TestHelper.GetJohnsKycDocument();

                Assert.NotNull(kycDocument);
                Assert.True(kycDocument.Id.Length > 0);
                Assert.True(kycDocument.Status == KycStatus.CREATED);
                Assert.True(kycDocument.Type == KycDocumentType.IDENTITY_PROOF);
            }
            catch (Exception ex)
            {
                Assert.True(false, ex.Message);
            }
        }

        [Fact]
        public async Task Test_Users_SaveKycDocument()
        {
            try
            {
                UserNaturalDTO john = TestHelper.GetJohn();
                KycDocumentDTO kycDocument = TestHelper.GetJohnsKycDocument();

                Assembly assembly = Assembly.GetExecutingAssembly();
                FileInfo assemblyFileInfo = new FileInfo(assembly.Location);
                FileInfo fi = assemblyFileInfo.Directory.GetFiles("TestKycPageFile.png").Single();

                await _objectToTest.CreateKycPage(john.Id, kycDocument.Id, fi.FullName);

                KycDocumentPutDTO kycDocumentPut = new KycDocumentPutDTO
                {
                    Status = KycStatus.VALIDATION_ASKED
                };

                KycDocumentDTO result = _objectToTest.UpdateKycDocument(john.Id, kycDocumentPut, kycDocument.Id).Result;

                Assert.NotNull(result);
                Assert.True(kycDocument.Type == result.Type);
                Assert.True(result.Status == KycStatus.VALIDATION_ASKED);
            }
            catch (Exception ex)
            {
                Assert.True(false, ex.Message);
            }
        }

        [Fact]
        public void Test_Users_GetKycDocument()
        {
            try
            {
                UserNaturalDTO john = TestHelper.GetJohn();
                KycDocumentDTO kycDocument = TestHelper.GetJohnsKycDocument();

                KycDocumentDTO result = _objectToTest.GetKycDocument(john.Id, kycDocument.Id).Result;

                Assert.NotNull(result);
                Assert.True(kycDocument.Id == (result.Id));
                Assert.True(kycDocument.Type == (result.Type));
                Assert.True(kycDocument.CreationDate == result.CreationDate);
            }
            catch (Exception ex)
            {
                Assert.True(false, ex.Message);
            }
        }

        [Fact]
        public async Task Test_Users_CreateKycPageFromFile()
        {
            try
            {
                UserNaturalDTO john = TestHelper.GetJohn();
                KycDocumentDTO kycDocument = TestHelper.GetNewKycDocument();

                Assembly assembly = Assembly.GetExecutingAssembly();
                FileInfo assemblyFileInfo = new FileInfo(assembly.Location);
                FileInfo fi = assemblyFileInfo.Directory.GetFiles("TestKycPageFile.png").Single();

                await _objectToTest.CreateKycPage(john.Id, kycDocument.Id, fi.FullName);
            }
            catch (Exception ex)
            {
                Assert.True(false, ex.Message);
            }
        }

        [Fact]
        public async Task Test_Users_CreateKycPageFromBytes()
        {
            try
            {
                UserNaturalDTO john = TestHelper.GetJohn();
                KycDocumentDTO kycDocument = TestHelper.GetNewKycDocument();

                Assembly assembly = Assembly.GetExecutingAssembly();
                FileInfo assemblyFileInfo = new FileInfo(assembly.Location);
                FileInfo fi = assemblyFileInfo.Directory.GetFiles("TestKycPageFile.png").Single();
                byte[] bytes = File.ReadAllBytes(fi.FullName);

                await _objectToTest.CreateKycPage(john.Id, kycDocument.Id, bytes);
            }
            catch (Exception ex)
            {
                Assert.True(false, ex.Message);
            }
        }

        [Fact]
        public void Test_Users_AllCards()
        {
            try
            {
                UserNaturalDTO john = TestHelper.GetJohn();
                PayInCardDirectDTO payIn = TestHelper.GetNewPayInCardDirect();
                Pagination pagination = new Pagination(1, 1);
                CardDTO card = TestHelper.CardsApi.Get(payIn.CardId).Result;
                ListPaginated<CardDTO> cards = _objectToTest.GetCards(john.Id, pagination).Result;

                Assert.True(cards.Count == 1);
                Assert.True(cards[0].CardType != CardType.NotSpecified);
                TestHelper.AssertEqualInputProps(cards[0], card);


                // test sorting
                ListPaginated<CardDTO> result = null;
                ListPaginated<CardDTO> result2 = null;

                pagination = new Pagination(1, 2);
                Sort sort = new Sort();
                sort.AddField("CreationDate", SortDirection.asc);
                result = _objectToTest.GetCards(john.Id, pagination, sort).Result;
                Assert.NotNull(result);
                Assert.True(result.Count > 0);

                sort = new Sort();
                sort.AddField("CreationDate", SortDirection.desc);
                result2 = _objectToTest.GetCards(john.Id, pagination, sort).Result;
                Assert.NotNull(result2);
                Assert.True(result2.Count > 0);

                Assert.True(result[0].Id != result2[0].Id);
            }
            catch (Exception ex)
            {
                Assert.True(false, ex.Message);
            }
        }

        [Fact]
        public void Test_Users_Transactions()
        {
            try
            {
                UserNaturalDTO john = TestHelper.GetJohn();
                TransferDTO transfer = TestHelper.GetNewTransfer();
                Pagination pagination = new Pagination(1, 1);

                ListPaginated<TransactionDTO> transactions = _objectToTest.GetTransactions(john.Id, pagination, new FilterTransactions()).Result;

                Assert.True(transactions.Count > 0);


                // test sorting
                ListPaginated<TransactionDTO> result = null;
                ListPaginated<TransactionDTO> result2 = null;

                pagination = new Pagination(1, 2);
                Sort sort = new Sort();
                sort.AddField("CreationDate", SortDirection.asc);
                result = _objectToTest.GetTransactions(john.Id, pagination, new FilterTransactions(), sort).Result;
                Assert.NotNull(result);
                Assert.True(result.Count > 0);

                sort = new Sort();
                sort.AddField("CreationDate", SortDirection.desc);
                result2 = _objectToTest.GetTransactions(john.Id, pagination, new FilterTransactions(), sort).Result;
                Assert.NotNull(result2);
                Assert.True(result2.Count > 0);

                Assert.True(result[0].Id != result2[0].Id);
            }
            catch (Exception ex)
            {
                Assert.True(false, ex.Message);
            }
        }

        [Fact]
        public void Test_Users_GetKycDocuments()
        {
            ListPaginated<KycDocumentDTO> result = null;

            UserNaturalDTO john = TestHelper.GetJohn();
            KycDocumentDTO kycDocument = TestHelper.GetJohnsKycDocument();

            try
            {
                result = _objectToTest.GetKycDocuments(john.Id, null, null).Result;

                Assert.NotNull(result);
                Assert.True(result.Count > 0);


                // test sorting
                TestHelper.GetNewKycDocument();
                result = null;
                ListPaginated<KycDocumentDTO> result2 = null;

                Pagination pagination = new Pagination(1, 2);
                Sort sort = new Sort();
                sort.AddField("CreationDate", SortDirection.asc);
                result = _objectToTest.GetKycDocuments(john.Id, pagination, null, sort).Result;
                Assert.NotNull(result);
                Assert.True(result.Count > 0);

                sort = new Sort();
                sort.AddField("CreationDate", SortDirection.desc);
                result2 = _objectToTest.GetKycDocuments(john.Id, pagination, null, sort).Result;
                Assert.NotNull(result2);
                Assert.True(result2.Count > 0);

                Assert.True(result[0].Id != result2[0].Id);
            }
            catch (Exception ex)
            {
                Assert.True(false, ex.Message);
            }
        }

        [Fact]
        public async Task Test_Users_GetEmoney()
        {
            try
            {
                var user = TestHelper.GetNewJohn();
                var wallet = TestHelper.GetNewJohnsWalletWithMoney(10000, user);

                var emoney = await _objectToTest.GetEmoney(user.Id);

                Assert.Equal(user.Id, emoney.UserId);
                Assert.Equal(10000, emoney.CreditedEMoney.Amount);
                Assert.Equal(CurrencyIso.EUR, emoney.CreditedEMoney.Currency);
            }
            catch (Exception ex)
            {
                Assert.True(false, ex.Message);
            }
        }

        [Fact]
        public void Test_Users_GetEmoneyWithCurrency()
        {
            try
            {
                var user = TestHelper.GetNewJohn();
                var wallet = TestHelper.GetNewJohnsWalletWithMoney(10000, user);

                var emoney = _objectToTest.GetEmoney(user.Id, CurrencyIso.USD).Result;

                Assert.Equal(user.Id, emoney.UserId);
                Assert.Equal(CurrencyIso.USD, emoney.CreditedEMoney.Currency);
            }
            catch (Exception ex)
            {
                Assert.True(false, ex.Message);
            }
        }

        [Fact]
        public void Test_Users_GetTransactionsForBankAccount()
        {
            try
            {
                var payOut = TestHelper.GetJohnsPayOutBankWire();
                string bankAccountId = TestHelper.GetJohnsAccount().Id;

                var pagination = new Pagination(1, 1);
                var filter = new FilterTransactions();
                var sort = new Sort();
                sort.AddField("CreationDate", SortDirection.desc);

                var transactions = _objectToTest.GetTransactionsForBankAccount(bankAccountId, pagination, filter, sort).Result;

                Assert.True(transactions.Count > 0);
            }
            catch (Exception ex)
            {
                Assert.True(false, ex.Message);
            }
        }
    }
}
