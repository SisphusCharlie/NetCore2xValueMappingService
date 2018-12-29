using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using RawRabbit;
using RawRabbit.Context;
using RawRabbit.vNext;
using StackExchange.Redis;
using ValueMappingCoreAPI.Custome.ExceptionHandler;
using ValueMappingCoreAPI.CustomeFilter;
using ValueMappingCoreAPI.Extensions;
using ValueMappingCoreAPI.Models;
using ValueMappingCoreAPI.Models.DashBoard;
using ValueMappingCoreAPI.Models.MapViewModels;
using ValueMappingCoreAPI.Options;
using ValueMappingCoreAPI.Repository;
using ValueMappingCoreAPI.Services;
using ValueMappingCoreAPI.Services.CacheHelper;
using ValueMappingCoreAPI.Services.RedisHelper;

namespace ValueMappingCoreAPI.Controllers
{
    //[Authorize]
    //[AddHeader("Auther", "CharlieHuangx@163.com")]
    public class ValueMapsController : Controller
    {
        private ISystemRepository systemRepository;
        private IValueMapsRepository valuemapsRepository;
        private readonly IEmailSender emailSender;
        //IDistributedCache redisCache;
        public static string ListKeyName = "SystemList";
        public static string AddedListName = "AddedList";
        private static readonly string connect= "127.0.0.1:6379,abortConnect = false,connectRetry = 3,connectTimeout = 3000,defaultDatabase = 1,syncTimeout = 3000,version = 3.2.1,responseTimeout = 3000";
        private static Lazy<ConnectionMultiplexer> lazyRedisConnection = new Lazy<ConnectionMultiplexer>(() =>
        {
            return ConnectionMultiplexer.Connect(connect);
        });
        private IMemoryCache memoryCache;
        //private readonly ICapPublisher _publisher;
        public static ConnectionMultiplexer RedisConnection
        {
            get
            {
                return lazyRedisConnection.Value;
            }
        }
        //RedisService redissv;
        private readonly IRedisServiceHelper _redisService;
        ConcurrentDictionary<Guid, string> boardmsgs = new ConcurrentDictionary<Guid, string>();
        private static ConcurrentQueue<BoardMessage> boardmsgsque = new ConcurrentQueue<BoardMessage>();
        //RabbitMqOptions rabbitmq=new RabbitMqOptions();
        private static IBusClient busClient;
        private IDistributedCache distcache;
        private static List<BoardMessage> listbm = new List<BoardMessage>();
        public ValueMapsController(ISystemRepository sr, IValueMapsRepository vmr, IEmailSender es, IRedisServiceHelper redisService, IMemoryCache mCache,IDistributedCache discache)
        {
            systemRepository = sr;
            valuemapsRepository = vmr;
            emailSender = es;
            _redisService = redisService;
            memoryCache = mCache;
            distcache = discache;
            //rabbitmq = rabbit;
            ////redisCache=rc;
            //_publisher = icp;
        }


        [Route("Index")]
        //[HttpGet(Name ="ValueMapList")]
        [ShortCircuitingFilterAttribute]
        [TypeFilter(typeof(AddHeaderAttribute), Arguments = new object[] { "Auther", "CharlieHuangx@163.com" })]
        public async Task<IActionResult> Index(int pageindex = 1)
        {
            var pagesize = 5;
            var notes = valuemapsRepository.PageList(pageindex, pagesize);
            ViewBag.PageCount = notes.Item2;
            ViewBag.PageIndex = pageindex;

            var sys = await systemRepository.ListAsync();
            ViewBag.SysTypes = sys.Select(r => new SelectListItem
            {
                Text = r.SystemName,
                Value = r.Id.ToString()
            });
            var temp = from q in sys
                       select q.SystemName;
            string cachevaluestring = "X";
            string cachevalue = string.Join(",", temp);

            string inner = await _redisService.GetAsync($"SystemMap:{cachevaluestring}");
            if (string.IsNullOrEmpty(inner))
            {
                //var one = sys.Where((x) => x.Id > x.Id).First();
                await _redisService.SetAsync($"SystemMap:{cachevaluestring}", JsonConvert.SerializeObject(cachevalue));
            }


            //redis distributed cache
            //string cachevaluestring = "xxx";
            //OperateSystem opone = new OperateSystem();
            //var dissystemcache = await distcache.GetStringAsync($"SystemMap:{cachevaluestring}");
            //if (dissystemcache != null)
            //{
            //    opone = JsonConvert.DeserializeObject<OperateSystem>(dissystemcache);

            //}
            //else
            //{
            //    opone = sys.Where((x)=>x.Id>x.Id).First();
            //    var invalue = JsonConvert.SerializeObject(opone);
            //    await distcache.SetStringAsync(
            //        $"SystemMap:{cachevaluestring}",
            //        invalue,
            //        new DistributedCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(1))
            //        );
            //}




            //await _redisService.SetAsync("AllSystem", string.Join(",", temp));


            //HttpContext.Session.SetString(ListKeyName, JsonConvert.SerializeObject(sys));

            //var temp=HttpContext.Session.GetString(ListKeyName);
            //if (redissv == null)
            //{
            //    redissv = new RedisService();
            //}
            //redisCache.
            //redisCache.Set(ListKeyName,SystemExtend.SerializeToBuffer<List<OperateSystem>>(sys));
            //var getcache = redisCache.Get(ListKeyName);
            //List<OperateSystem> a = new List<OperateSystem>();
            //var end = SystemExtend.DesrializeFromBuffer<List<OperateSystem>>(a, getcache);
            //CacheUntity.SetCache(ListKeyName, SystemExtend.SerializeToBuffer<List<OperateSystem>>(sys));
            //CacheUntity.GetCache(ListKeyName);

            //var res = CacheUntity.GetCache<List<OperateSystem>>(ListKeyName);
            //  var db = RedisConnection.GetDatabase();

            //  if (db.IsConnected(AddedListName) && (!db.KeyExists(AddedListName) || !db.KeyType(AddedListName).Equals(RedisType.List)))
            //  {
            //      //Add sample data.
            //      //db.KeyDelete(AddedListName);
            //      //db.KeyExists(AddedListName);
            //      //Push data from the left
            //      db.SetAdd(AddedListName,JsonConvert.SerializeObject(sys));


            //  }
            //var aaa=  db.StringGet(AddedListName);
            return View(notes.Item1);
        }

        public static void AddRedisData(byte[] input)
        {
            var db = RedisConnection.GetDatabase();

            if (db.IsConnected(AddedListName) && (!db.KeyExists(AddedListName) || !db.KeyType(AddedListName).Equals(RedisType.List)))
            {
                //Add sample data.
                //db.KeyDelete(AddedListName);
                //db.KeyExists(AddedListName);
                //Push data from the left
                db.ListLeftPushAsync(AddedListName, input);
                
            }
        }
        public static void GetRedisSendData(byte[] input)
        {
            var db = RedisConnection.GetDatabase();

            if (db.IsConnected(AddedListName) && (!db.KeyExists(AddedListName) || !db.KeyType(AddedListName).Equals(RedisType.List)))
            {
               var each=db.ListRightPopAsync(AddedListName);

               var predata= each.Result as object;
               var aa= predata as ValueMaps;
                aa.SetJSONString();
            }
        }

        public async Task<IActionResult> Add()
        {
          //var listselect1=  HttpContext.Session.GetString(ListKeyName);

          // var listselect= JsonConvert.DeserializeObject<List<OperateSystem>>(listselect1.ToString());
          //  if (listselect == null)
          //  {
                var sys = await systemRepository.ListAsync();
                ViewBag.SysTypes = sys.Select(r => new SelectListItem
                {
                    Text = r.SystemName,
                    Value = r.Id.ToString()
                });
            //    HttpContext.Session.SetString(ListKeyName, JsonConvert.SerializeObject(sys));
            //}
            //else
            //{
            //    ViewBag.SysTypes = listselect.Select(r => new SelectListItem
            //    {
            //        Text = r.SystemName,
            //        Value = r.Id.ToString()
            //    });
            //}


            return View();
        }


        [ExceptionHandler]
        [HttpPost]
        public async Task<IActionResult> Add([FromServices]IHostingEnvironment env, ValueMapsModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var auth = await HttpContext.AuthenticateAsync();
            string currentUser = "";
            string currentip = ContextIP.GetUserIp(this.HttpContext);
            if (auth.Succeeded)
            {
                currentUser = auth.Principal.Identity.Name;

            }


            string filename = string.Empty;
            ValueMaps tempin = new ValueMaps
            {
                SystemId = model.System,
                ValuationFunction = model.ValuationFunction,
                TransdateTime = DateTime.Now,
                Threshold = model.Threshold,
                Uid = currentUser
            };
            await valuemapsRepository.AddAsync(tempin);

            //redis
           await _redisService.SetAsync("First",tempin.ToString());



            StringBuilder sb = new StringBuilder();
            sb.Append("<table class='table' style='border: solid; color: silver'>" 
                + "<thead style='border: solid; color: red'>" + "< tr>< th > User </ th >< th > Datatime </ th ></ tr ></thead>" +
                    "<tbody style='border: solid; color: goldenrod'>"
                  + "<tr>< td >" + currentUser+" </ td >< td >"+ DateTime.Now+" </ td ></ tr >< tr > "
        
        +"</table>");
            sb.AppendLine(tempin.SetJSONString());
            //byte[] tempssin = tempin.SetBinary<ValueMaps>();
            //AddRedisData(tempssin);

            //pubish redis message
            //using (ConnectionMultiplexer redis = ConnectionMultiplexer.Connect("127.0.0.1:6379"))
            //{
            //    ISubscriber sub = redis.GetSubscriber();
            //  var rel=  sub.Publish("Addmessages", tempin.SetJSONString());
            //}


            //var busConfig = new RawRabbitConfiguration
            //rabbitmq
            //= new RabbitMqOptions
            //{
            //    Username = "guest",
            //    Password = "guest",
            //    Port = 5672,
            //    VirtualHost = "/",
            //    Hostnames = { "localhost" }

            //};
            //var busClient = BusClientFactory.CreateDefault(rabbitmq);
            busClient = BusClientFactory.CreateDefault(new RabbitMqOptions());
            //using (var busClient = BusClientFactory.CreateDefault(new RabbitMqOptions()))
            //{
                await busClient.PublishAsync<string>(tempin.SetJSONString());
            //}


            //await  emailSender.SendEmailAsync(currentUser, "Added one record!", sb.ToString());
            //await  valuemapsRepository.PublishAsync(tempin);
            return RedirectToAction("Index");
        }

        [ResponseCache(CacheProfileName ="Normal")]
        [HttpGet("DashBoard")]
        public async Task<IActionResult> DashBoard()
        {
            //List<string> dash = new List<string>();
            await ProcessRabbitMsg();
            //while (boardmsgsque.Count > 0)
            //{
            //    BoardMessage eachbm = new BoardMessage();
            //    boardmsgsque.TryDequeue(out eachbm);
            //    if (eachbm != null)
            //    {
            //        listbm.Add(eachbm);
            //    }
            //}
            //redis
            //using (ConnectionMultiplexer redis = ConnectionMultiplexer.Connect("127.0.0.1:6379"))
            //{
            //    ISubscriber sub = redis.GetSubscriber();    //订阅名为 messages 的通道

            //    sub.Subscribe("Addmessages", (channel, message) => {        //输出收到的消息

            //        dash.Add(message.ToString());
            //        //Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] {message}");
            //    });
            //}

            //rabbitmq = new RabbitMqOptions
            //{
            //    Username = "guest",
            //    Password = "guest",
            //    Port = 5672,
            //    VirtualHost = "/",
            //    Hostnames = { "localhost" }
            //};



            //busClient.SubscribeAsync<string>((msg, context)=>func(msg, context));
            //=>
            //{
            // Task tk= Task.Run(() =>
            //        {
            //            dash.Add(msg);

            //        });
            //    return tk;
            //});

            //boardmsgsque.TryDequeue();
            //ViewBag.ResultList = dash;
            return View(listbm);
        }


        //public async Task<IActionResult> RealtimeDashboard()
        //{

        //    return View();
        //}


        public async Task ProcessRabbitMsg()
        {
            //var busClient = BusClientFactory.CreateDefault(rabbitmq);
            busClient = BusClientFactory.CreateDefault(new RabbitMqOptions());
            //ReaderWriterLockSlim lockSlim = new ReaderWriterLockSlim();
            Func<string, MessageContext, Task> func =
              async delegate (string x, MessageContext y)
              {
                  Task tk = Task.Factory.StartNew(() =>
                  {
                      //boardmsgs.AddOrUpdate(y.GlobalRequestId,key=>x,(key,oldvalue)=>x);
                      //boardmsgsque.Append<BoardMessage>(new BoardMessage { MsgID = y.GlobalRequestId, MsgContent = x });
                      listbm.Add(new BoardMessage { MsgID = y.GlobalRequestId, MsgContent = x });
                      //boardmsgsque.Enqueue(new BoardMessage { MsgID = y.GlobalRequestId, MsgContent = x });
                  });
                  //tk.Wait();
                  await tk;
              };
            //lockSlim.EnterWriteLock();
            //ViewData["result“]
            Task tkout = Task.Run(
                () =>
                {
                    //lockSlim.EnterWriteLock();

                        busClient.SubscribeAsync<string>((msg, context) => func(msg, context));
                        //lockSlim.ExitWriteLock();
                }

                );
           await tkout;
        }




        //public async Task<IActionResult> PublishMessageWithTransaction([FromServices]Data.ApplicationDbContext dbContext,object obj)
        //{
        //    using (var trans = dbContext.Database.BeginTransaction())
        //    {
        //        //指定发送的消息标题（供订阅）和内容
        //        await _publisher.PublishAsync("xxx.services.account.check",
        //            new Person { Name = "Foo", Age = 11 });
        //        // 你的业务代码。
        //        trans.Commit();
        //    }
        //    return Ok();
        //}
        //[NonAction]
        //[CapSubscribe("xxx.services.account.check")]
        //public async Task CheckReceivedMessage(ValueMaps vm)
        //{
        //    await Task.CompletedTask;
        //}
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var vm = await valuemapsRepository.GetByIdAsync(id);
            //if (!string.IsNullOrEmpty(note.Password))
            //    return View();
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Details(int id, string password)
        {
            var vm = await valuemapsRepository.GetByIdAsync(id);
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Edit([FromServices]IHostingEnvironment env, ValueMaps model)
        {
            ValueMaps updated = new ValueMaps();
            if (ModelState.IsValid)
            {
                var auth = await HttpContext.AuthenticateAsync();
                string currentUser = "";
                string currentip = ContextIP.GetUserIp(this.HttpContext);
                if (auth.Succeeded)
                {
                    currentUser = auth.Principal.Identity.Name;
                }
                updated = await valuemapsRepository.GetByIdAsync(model.Id);

                updated.SystemId = model.SystemId;
                updated.Threshold = model.Threshold;
                updated.ValuationFunction = model.ValuationFunction;
                updated.Uid = currentUser;
                updated.TransdateTime = DateTime.Now;
                var mapping = systemRepository.FindById(model.SystemId);
                updated.System = mapping;


                await valuemapsRepository.UpdateAsync(updated);
            }
            else
            {
                return BadRequest("Something is wrong,please retry");
            }
            //if (!note.Password.Equals(password))

            return View("Details", updated);
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            //var inidata1 = HttpContext.Session.GetString(ListKeyName);
            //var inidata = JsonConvert.DeserializeObject<List<OperateSystem>>(inidata1);
           //var lastone=await _redisService.GetAsync("First");
            var lastone=await _redisService.GetAsync($"SystemMap:X");

            ViewBag.AllSelect = lastone;
            //ViewBag.LastOne=
            var sys = await systemRepository.ListAsync();
            ViewBag.SysTypes = sys.Select(r => new SelectListItem
            {
                Text = r.SystemName,
                Value = r.Id.ToString()
            });

            var vm = await valuemapsRepository.GetByIdAsync(id);
            return View("Edit",vm);
        }


        //[HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await valuemapsRepository.DeleteAsync(id);
            return RedirectToAction("Index");
        }

        [HttpGet("Stepin")]
        public IActionResult Stepin()
        {
            //var desurl = Url.Action("Buy","Products",new { id=17,color="Red"},protocol:Request.Scheme);
            var desurl = Url.RouteUrl("Destination_Route");
            return Content($"Go to {desurl} please");
        }
        //[HttpGet("custome/url/to/out")]
        [HttpGet("custome/url/to/out", Name = "Destination_Route")]
        public IActionResult StepOut()
        {
            return View();
        }

        [AcceptVerbs("Get", "Post")]
        public IActionResult VerifyVF(string vf)
        {
            if (vf.Contains("X"))
            {
                return Json(data: $"This VF {vf} is forbided!");
            }
            return Json(data: true);
        }

        public IActionResult CacheRelated()
        {
            string cachekey = "key";
            string result;
            if (!memoryCache.TryGetValue(cachekey, out result))
            {
                result = $"LineZero{DateTime.Now}";
                memoryCache.Set(cachekey, result);

                //相对过期
                memoryCache.Set(cachekey, result,
                    new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromMinutes(2)));
                //绝对过期
                memoryCache.Set(cachekey, result,
                    new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(2)));
                //移除
                memoryCache.Remove(cachekey);
                //优先级
                memoryCache.Set(cachekey, result,
                  new MemoryCacheEntryOptions().SetPriority(CacheItemPriority.NeverRemove));

                //过期回调
                memoryCache.Set(cachekey, result,
                  new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(2))
                  .RegisterPostEvictionCallback
                  ((key, value, reason, substate) =>
                  {
                      Console.WriteLine($"键{key}值{value}改变,因为{reason}");
                  }));

                //缓存回调，在token过期时
                var cts = new CancellationTokenSource();
                memoryCache.Set(cachekey, result,
                  new MemoryCacheEntryOptions()
                  .AddExpirationToken(new CancellationChangeToken(cts.Token))
                  .RegisterPostEvictionCallback
                  ((key, value, reason, substate) =>
                  {
                      Console.WriteLine($"键{key}值{value}改变,因为{reason}");
                  }));
                cts.Cancel();
            }
            ViewBag.Cache = result;
            return View();
        }
    }
}