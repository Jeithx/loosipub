using AutoMapper;
using Core.Models.Read;
using Core.Models.Write;
using Entities.Models;

namespace Business.AutoMapperProfiles
{
    public class Profiles : Profile
    {
        public Profiles()
        {
            CreateMap<Entities.Models.User, UserRD>().ReverseMap().MaxDepth(1);
            CreateMap<Entities.Models.User, UserWD>().ReverseMap().MaxDepth(1);
            CreateMap<UserRD, UserWD>().ReverseMap().MaxDepth(1);

            CreateMap<Country, CountryRD>().ReverseMap().MaxDepth(1);
            CreateMap<Country, CountryWD>().ReverseMap().MaxDepth(1);
            CreateMap<CountryRD, CountryWD>().ReverseMap().MaxDepth(1);

            CreateMap<City, CityRD>().ReverseMap().MaxDepth(1);
            CreateMap<City, CityWD>().ReverseMap().MaxDepth(1);
            CreateMap<CityRD, CityWD>().ReverseMap().MaxDepth(1);

            CreateMap<Gender, GenderRD>().ReverseMap().MaxDepth(1);
            CreateMap<Gender, GenderWD>().ReverseMap().MaxDepth(1);
            CreateMap<GenderRD, GenderWD>().ReverseMap().MaxDepth(1);

            CreateMap<BlockedUser, BlockedUserRD>().ReverseMap().MaxDepth(1);
            CreateMap<BlockedUser, BlockedUserWD>().ReverseMap().MaxDepth(1);
            CreateMap<BlockedUserRD, BlockedUserWD>().ReverseMap().MaxDepth(1);

            CreateMap<Message, MessageRD>().ReverseMap().MaxDepth(1);
            CreateMap<Message, MessageWD>().ReverseMap().MaxDepth(1);
            CreateMap<MessageRD, MessageWD>().ReverseMap().MaxDepth(1);

            CreateMap<PostComment, PostCommentRD>().ReverseMap().MaxDepth(1);
            CreateMap<PostComment, PostCommentWD>().ReverseMap().MaxDepth(1);
            CreateMap<PostCommentRD, PostCommentWD>().ReverseMap().MaxDepth(1);

            CreateMap<Post, PostRD>().ForMember(dest => dest.UserDisplayName,
                    opt => opt.MapFrom(src =>
                        src.User != null ? src.User.DisplayName : ""))
                .ForMember(dest => dest.UserName,
                    opt => opt.MapFrom(src =>
                        src.User != null ? src.User.UserName : ""))
                .ForMember(dest => dest.UserProfilePicture,
                    opt => opt.MapFrom(src =>
                        src.User != null ? src.User.ProfilePictureUrl : ""))
                .ReverseMap().MaxDepth(1);
            CreateMap<Post, PostWD>().ReverseMap().MaxDepth(1);
            CreateMap<PostRD, PostWD>().ReverseMap().MaxDepth(1);
            CreateMap<PostRD, PostUpdateWD>().ReverseMap().MaxDepth(1);
            CreateMap<PostUpdateWD, PostWD>().ReverseMap().MaxDepth(1);

            CreateMap<PostLike, PostLikeRD>().ReverseMap().MaxDepth(1);
            CreateMap<PostLike, PostLikeWD>().ReverseMap().MaxDepth(1);
            CreateMap<PostLikeRD, PostLikeWD>().ReverseMap().MaxDepth(1);

            CreateMap<PostMedia, PostMediaRD>().ReverseMap().MaxDepth(1);
            CreateMap<PostMedia, PostMediaWD>().ReverseMap().MaxDepth(1);
            CreateMap<PostMediaRD, PostMediaWD>().ReverseMap().MaxDepth(1);

            CreateMap<PostTag, PostTagRD>()
                .ForMember(dest => dest.Tag,
                    opt => opt.MapFrom(src =>
                        src.Tag != null ? src.Tag.Name : ""))
                .ReverseMap().MaxDepth(1);
            CreateMap<PostTag, PostTagWD>().ReverseMap().MaxDepth(1);
            CreateMap<PostTagRD, PostTagWD>().ReverseMap().MaxDepth(1);

            CreateMap<Tag, TagRD>().ReverseMap().MaxDepth(1);
            CreateMap<Tag, TagWD>().ReverseMap().MaxDepth(1);
            CreateMap<TagRD, TagWD>().ReverseMap().MaxDepth(1);

            CreateMap<Report, ReportRD>().ReverseMap().MaxDepth(1);
            CreateMap<Report, ReportWD>().ReverseMap().MaxDepth(1);
            CreateMap<ReportRD, ReportWD>().ReverseMap().MaxDepth(1);

            CreateMap<SocialMedia, SocialMediaRD>().ReverseMap().MaxDepth(1);
            CreateMap<SocialMedia, SocialMediaWD>().ReverseMap().MaxDepth(1);
            CreateMap<SocialMediaRD, SocialMediaWD>().ReverseMap().MaxDepth(1);

            CreateMap<UserSocialMedia, UserSocialMediaRD>()
                .ForMember(dest => dest.SocialMediaName,
                    opt => opt.MapFrom(src =>
                        src.SocialMedia != null ? src.SocialMedia.Name : ""))
                .ForMember(dest => dest.SocialMediaLogo,
                    opt => opt.MapFrom(src =>
                        src.SocialMedia != null ? src.SocialMedia.Logo : "")).ReverseMap().MaxDepth(1);
            CreateMap<UserSocialMedia, UserSocialMediaWD>().ReverseMap().MaxDepth(1);
            CreateMap<UserSocialMediaRD, UserSocialMediaWD>().ReverseMap().MaxDepth(1);

            CreateMap<Subscriber, SubscriberRD>().ReverseMap().MaxDepth(1);
            CreateMap<Subscriber, SubscriberWD>().ReverseMap().MaxDepth(1);
            CreateMap<SubscriberRD, SubscriberWD>().ReverseMap().MaxDepth(1);

            CreateMap<Faq, FaqRD>().ForMember(dest => dest.Language,
                opt => opt.MapFrom(src =>
                    src.Language != null ? src.Language.Name : "")).ReverseMap().MaxDepth(1);
            CreateMap<Faq, FaqWD>().ReverseMap().MaxDepth(1);
            CreateMap<FaqRD, FaqWD>().ReverseMap().MaxDepth(1);

            CreateMap<UserFollower, UserFollowerRD>().ReverseMap().MaxDepth(1);
            CreateMap<UserFollower, UserFollowerWD>().ReverseMap().MaxDepth(1);
            CreateMap<UserFollowerRD, UserFollowerWD>().ReverseMap().MaxDepth(1);

            CreateMap<Language, LanguageRD>().ReverseMap().MaxDepth(1);
            CreateMap<Language, LanguageWD>().ReverseMap().MaxDepth(1);
            CreateMap<LanguageRD, LanguageWD>().ReverseMap().MaxDepth(1);

            CreateMap<Wallet, WalletRD>().ReverseMap().MaxDepth(1);
            CreateMap<Wallet, WalletWD>().ReverseMap().MaxDepth(1);
            CreateMap<WalletRD, WalletWD>().ReverseMap().MaxDepth(1);

            CreateMap<WalletLog, WalletLogRD>().ReverseMap().MaxDepth(1);
            CreateMap<WalletLog, WalletLogWD>().ReverseMap().MaxDepth(1);
            CreateMap<WalletLogRD, WalletLogWD>().ReverseMap().MaxDepth(1);

            CreateMap<Notification, NotificationRD>().ReverseMap().MaxDepth(1);
            CreateMap<Notification, NotificationWD>().ReverseMap().MaxDepth(1);
            CreateMap<NotificationRD, NotificationWD>().ReverseMap().MaxDepth(1);

            CreateMap<UserCallPrice, UserCallPriceRD>().ForMember(dest => dest.DisplayName,
                    opt => opt.MapFrom(src =>
                        src.User != null ? src.User.DisplayName : ""))
                .ForMember(dest => dest.Username,
                    opt => opt.MapFrom(src =>
                        src.User != null ? src.User.UserName : ""))
                .ForMember(dest => dest.ProfilePhotoUrl,
                    opt => opt.MapFrom(src =>
                        src.User != null ? src.User.ProfilePictureUrl : ""))
                      .ForMember(dest => dest.CoverPhotoUrl,
                    opt => opt.MapFrom(src =>
                        src.User != null ? src.User.CoverPictureUrl : ""))
                .ReverseMap().MaxDepth(1);
            CreateMap<UserCallPrice, UserCallPriceWD>().ReverseMap().MaxDepth(1);
            CreateMap<UserCallPriceRD, UserCallPriceWD>().ReverseMap().MaxDepth(1);

            CreateMap<Call, CallRD>()
                .ForMember(dest => dest.CallerUsername,
                    opt => opt.MapFrom(src =>
                        src.CallerUser != null ? src.CallerUser.UserName : ""))
                .ForMember(dest => dest.CalledDisplayname,
                    opt => opt.MapFrom(src =>
                        src.CallerUser != null ? src.CallerUser.DisplayName : ""))
                .ForMember(dest => dest.ReceiverUsername,
                    opt => opt.MapFrom(src =>
                        src.ReceiverUser != null ? src.ReceiverUser.UserName : ""))
                .ForMember(dest => dest.ReceiverDisplayname,
                    opt => opt.MapFrom(src =>
                        src.ReceiverUser != null ? src.ReceiverUser.DisplayName : ""))
                .ReverseMap().MaxDepth(1);
            CreateMap<Call, CallWD>().ReverseMap().MaxDepth(1);
            CreateMap<CallRD, CallWD>().ReverseMap().MaxDepth(1);

            CreateMap<CallPayment, CallPaymentRD>()
                 .ForMember(dest => dest.CalledUsername,
                    opt => opt.MapFrom(src =>
                        src.Call != null ? src.Call.ReceiverUser.UserName : ""))
                .ForMember(dest => dest.CalledUserDisplayname,
                    opt => opt.MapFrom(src =>
                        src.Call != null ? src.Call.ReceiverUser.DisplayName : ""))
                .ReverseMap().MaxDepth(1);
            CreateMap<CallPayment, CallPaymentWD>().ReverseMap().MaxDepth(1);
            CreateMap<CallPaymentRD, CallPaymentWD>().ReverseMap().MaxDepth(1);

            CreateMap<UserLoginAttempt, UserLoginAttemptRD>().ReverseMap().MaxDepth(1);
            CreateMap<UserLoginAttempt, UserLoginAttemptWD>().ReverseMap().MaxDepth(1);
            CreateMap<UserLoginAttemptRD, UserLoginAttemptWD>().ReverseMap().MaxDepth(1);

            CreateMap<UserSubscriptionPlan, UserSubscriptionPlanRD>()
             .ForMember(dest => dest.Username,
                opt => opt.MapFrom(src =>
                    src.ContentCreatorUser != null ? src.ContentCreatorUser.UserName : ""))
            .ForMember(dest => dest.DisplayName,
                opt => opt.MapFrom(src =>
                    src.ContentCreatorUser != null ? src.ContentCreatorUser.DisplayName : ""))
            .ReverseMap().MaxDepth(1);
            CreateMap<UserSubscriptionPlan, UserSubscriptionPlanWD>().ReverseMap().MaxDepth(1);
            CreateMap<UserSubscriptionPlanRD, UserSubscriptionPlanWD>().ReverseMap().MaxDepth(1);
        }
    }
}