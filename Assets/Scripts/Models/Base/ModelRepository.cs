using System;
using System.Collections;
using System.Collections.Generic;

namespace Models.Base
{
    internal static class ModelRepository
    {
        private static readonly List<IModel> Models = new List<IModel>();

        public static TModel GetModel<TModel>() where TModel : IModel
        {
            TModel model =  Resolve<TModel>();
            
            if (model == null)
            {
                model = Register<TModel>();
            }
			
            return model;
        }
        
        private static TModel Register<TModel>(TModel model) where TModel : IModel
        {
            Models.Add(model);
            return model;
        }

        private static TModel Register<TModel>() where TModel : IModel
        {
            return Register(Activator.CreateInstance<TModel>());
        }

        private static TModel Resolve<TModel>() where TModel : IModel
        {
            IModel model = GetModelByType<TModel>();
            TModel typedModel = (TModel) model;
            return typedModel;
        }

        private static IModel GetModelByType<TModel>()
        {
            Type type = typeof(TModel);

            return Models.Find(model => IsModelOfType(model, type));
        }

        private static bool IsModelOfType(IModel model, Type modelType)
        {
            IList interfaces = model.GetType().GetInterfaces();
            bool a = interfaces.Contains(modelType);
            
            bool b = model.GetType() == modelType;

            return a || b;
        }
    }
}