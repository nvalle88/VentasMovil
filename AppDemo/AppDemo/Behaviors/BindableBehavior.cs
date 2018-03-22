﻿using System;
using Xamarin.Forms;

namespace AppDemo.Behaviors
{
    public class BindableBehavior<T> : Behavior<T> where T : BindableObject
    {
        public T LinkedElement { get; private set; }

        protected override void OnAttachedTo(T visualElement)
        {
            base.OnAttachedTo(visualElement);

            LinkedElement = visualElement;

            if (visualElement.BindingContext != null)

                BindingContext = visualElement.BindingContext;

            visualElement.BindingContextChanged += OnBindingContextChanged;
        }

        void OnBindingContextChanged(object sender, EventArgs e) => OnBindingContextChanged();

        protected override void OnDetachingFrom(T view)
        {
            view.BindingContextChanged -= OnBindingContextChanged;
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();

            BindingContext = LinkedElement.BindingContext;
        }
    }
}