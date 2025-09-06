using Autofac;
using Autofac.Extras.DynamicProxy;
using Business.Abstract;
using Business.Concrete;
using Castle.DynamicProxy;
using Core.Helper.Cache;
using Core.Utilities.Interceptors;
using Core.Utilities.Security.Jwt;
using DataAccess.Abstract;
using DataAccess.Concrete;
using System.Reflection;
using Module = Autofac.Module;

namespace Business.DependencyResolvers.Autofac
{
    public class AutofacBusinessModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<CacheService>().As<ICacheService>().SingleInstance();
            builder.RegisterType<JwtHelper>().As<ITokenHelper>(); builder.RegisterType<AuthService>().As<IAuthService>();

            builder.RegisterType<UserService>().As<IUserService>();
            builder.RegisterType<EFUserDAL>().As<IUserDAL>();

            builder.RegisterType<CountryService>().As<ICountryService>();
            builder.RegisterType<EFCountryDAL>().As<ICountryDAL>();

            builder.RegisterType<CityService>().As<ICityService>();
            builder.RegisterType<EFCityDAL>().As<ICityDAL>();

            builder.RegisterType<GenderService>().As<IGenderService>();
            builder.RegisterType<EFGenderDAL>().As<IGenderDAL>();

            builder.RegisterType<LanguageService>().As<ILanguageService>();
            builder.RegisterType<EFLanguageDAL>().As<ILanguageDAL>();

            builder.RegisterType<LogService>().As<ILogService>();
            builder.RegisterType<EFLogDAL>().As<ILogDAL>();

            builder.RegisterType<BlockedUserService>().As<IBlockedUserService>();
            builder.RegisterType<EFBlockedUserDAL>().As<IBlockedUserDAL>();

            builder.RegisterType<MessageService>().As<IMessageService>();
            builder.RegisterType<EFMessageDAL>().As<IMessageDAL>();

            builder.RegisterType<PostCommentService>().As<IPostCommentService>();
            builder.RegisterType<EFPostCommentDAL>().As<IPostCommentDAL>();

            builder.RegisterType<PostService>().As<IPostService>();
            builder.RegisterType<EFPostDAL>().As<IPostDAL>();

            builder.RegisterType<PostMediaService>().As<IPostMediaService>();
            builder.RegisterType<EFPostMediaDAL>().As<IPostMediaDAL>();

            builder.RegisterType<PostTagService>().As<IPostTagService>();
            builder.RegisterType<EFPostTagDAL>().As<IPostTagDAL>();

            builder.RegisterType<PostLikeService>().As<IPostLikeService>();
            builder.RegisterType<EFPostLikeDAL>().As<IPostLikeDAL>();

            builder.RegisterType<TagService>().As<ITagService>();
            builder.RegisterType<EFTagDAL>().As<ITagDAL>();

            builder.RegisterType<ReportService>().As<IReportService>();
            builder.RegisterType<EFReportDAL>().As<IReportDAL>();

            builder.RegisterType<SocialMediaService>().As<ISocialMediaService>();
            builder.RegisterType<EFSocialMediaDAL>().As<ISocialMediaDAL>();

            builder.RegisterType<UserSocialMediaService>().As<IUserSocialMediaService>();
            builder.RegisterType<EFUserSocialMediaDAL>().As<IUserSocialMediaDAL>();

            builder.RegisterType<SubscriberService>().As<ISubscriberService>();
            builder.RegisterType<EFSubscriberDAL>().As<ISubscriberDAL>();

            builder.RegisterType<FaqService>().As<IFaqService>();
            builder.RegisterType<EFFaqDAL>().As<IFaqDAL>();

            builder.RegisterType<UserFollowerService>().As<IUserFollowerService>();
            builder.RegisterType<EFUserFollowerDAL>().As<IUserFollowerDAL>();

            builder.RegisterType<WalletService>().As<IWalletService>();
            builder.RegisterType<EFWalletDAL>().As<IWalletDAL>();

            builder.RegisterType<WalletLogService>().As<IWalletLogService>();
            builder.RegisterType<EFWalletLogDAL>().As<IWalletLogDAL>();

            builder.RegisterType<NotificationService>().As<INotificationService>();
            builder.RegisterType<EFNotificationDAL>().As<INotificationDAL>();

            builder.RegisterType<UserCallPriceService>().As<IUserCallPriceService>();
            builder.RegisterType<EFUserCallPriceDAL>().As<IUserCallPriceDAL>();

            builder.RegisterType<CallService>().As<ICallService>();
            builder.RegisterType<EFCallDAL>().As<ICallDAL>();

            builder.RegisterType<CallPaymentService>().As<ICallPaymentService>();
            builder.RegisterType<EFCallPaymentDAL>().As<ICallPaymentDAL>();

            builder.RegisterType<UserLoginAttemptService>().As<IUserLoginAttemptService>();
            builder.RegisterType<EFUserLoginAttemptDAL>().As<IUserLoginAttemptDAL>();

            builder.RegisterType<UserSubscriptionPlanService>().As<IUserSubscriptionPlanService>();
            builder.RegisterType<EFUserSubscriptionPlanDAL>().As<IUserSubscriptionPlanDAL>();

            var assembly = Assembly.GetExecutingAssembly();
            builder.RegisterAssemblyTypes(assembly).AsImplementedInterfaces().EnableInterfaceInterceptors(new ProxyGenerationOptions()
            {
                Selector = new AspectInterceptorSelector()
            }).SingleInstance();
        }
    }
}
