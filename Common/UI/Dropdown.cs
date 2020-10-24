using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley;
using StardewValley.Menus;

namespace CJB.Common.UI
{
    /**


    This code is copied from Pathoschild.Stardew.Common.UI in https://github.com/Pathoschild/StardewMods,
    available under the MIT License. See that repository for the latest version.


    **/
    /// <summary>A button UI component which lets the player trigger a dropdown list.</summary>
    internal class Dropdown<TItem> : ClickableComponent
    {
        /*********
        ** Fields
        *********/
        /// <summary>The font with which to render text.</summary>
        private readonly SpriteFont Font;

        /// <summary>The dropdown list.</summary>
        private readonly DropdownList<TItem> List;

        /// <summary>The size of the rendered button borders.</summary>
        private readonly int BorderWidth = CommonSprites.Tab.TopLeft.Width * 2 * Game1.pixelZoom;


        /*********
        ** Accessors
        *********/
        /// <summary>Whether the dropdown list is expanded.</summary>
        public bool IsExpanded { get; set; }


        /*********
        ** Public methods
        *********/
        /// <summary>Construct an instance.</summary>
        /// <param name="x">The X-position at which to draw the tab.</param>
        /// <param name="y">The Y-position at which to draw the tab.</param>
        /// <param name="font">The font with which to render text.</param>
        /// <param name="selectedItem">The selected item.</param>
        /// <param name="items">The items in the list.</param>
        /// <param name="getLabel">Get the display label for an item.</param>
        public Dropdown(int x, int y, SpriteFont font, TItem selectedItem, TItem[] items, Func<TItem, string> getLabel)
            : base(Rectangle.Empty, getLabel(selectedItem))
        {
            this.Font = font;
            this.List = new DropdownList<TItem>(selectedItem, items, getLabel, x, y, font);
            this.bounds.X = x;
            this.bounds.Y = y;

            this.ReinitializeComponents();
        }

        /// <summary>Get whether the dropdown contains the given UI pixel position.</summary>
        /// <param name="x">The UI X position.</param>
        /// <param name="y">The UI Y position.</param>
        public override bool containsPoint(int x, int y)
        {
            return
                base.containsPoint(x, y)
                || (this.IsExpanded && this.List.containsPoint(x, y));
        }

        /// <summary>Select an item in the list if it's under the cursor.</summary>
        /// <param name="x">The X-position of the item in the UI.</param>
        /// <param name="y">The Y-position of the item in the UI.</param>
        /// <param name="selected">The selected item, if found.</param>
        /// <returns>Returns whether an item was selected.</returns>
        public bool TrySelect(int x, int y, out TItem selected)
        {
            if (this.IsExpanded)
                return this.List.TrySelect(x, y, out selected);

            selected = default;
            return false;
        }

        /// <summary>A method invoked when the player scrolls the dropdown using the mouse wheel.</summary>
        /// <param name="direction">The scroll direction.</param>
        public void ReceiveScrollWheelAction(int direction)
        {
            if (this.IsExpanded)
                this.List.ReceiveScrollWheelAction(direction);
        }

        /// <summary>Render the tab UI.</summary>
        /// <param name="sprites">The sprites to render.</param>
        /// <param name="opacity">The opacity at which to draw.</param>
        public void Draw(SpriteBatch sprites, float opacity = 1)
        {
            // draw tab
            CommonHelper.DrawButton(this.bounds.X, this.bounds.Y, this.List.MaxLabelWidth, this.List.MaxLabelHeight, out Vector2 textPos);
            sprites.DrawString(this.Font, this.List.SelectedLabel, textPos, Color.Black * opacity);

            // draw dropdown
            if (this.IsExpanded)
                this.List.Draw(sprites, opacity);
        }

        /// <summary>Recalculate dimensions and components for rendering.</summary>
        public void ReinitializeComponents()
        {
            this.bounds.Height = (int)this.Font.MeasureString("ABCDEFGHIJKLMNOPQRSTUVWXYZ").Y - 10 + this.BorderWidth; // adjust for font's broken measurement
            this.bounds.Width = this.List.MaxLabelWidth + this.BorderWidth;

            this.List.bounds.X = this.bounds.X;
            this.List.bounds.Y = this.bounds.Bottom;
            this.List.ReinitializeComponents();
        }
    }
}
