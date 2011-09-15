using System;
using Gwen.Control;

namespace Gwen.UnitTest
{
    public class MenuStrip : GUnit
    {
        public MenuStrip(Base parent)
            : base(parent)
        {
            Control.MenuStrip menu = new Control.MenuStrip(this);

            {
                Control.MenuItem pRoot = menu.AddItem("File");
                pRoot.Menu.AddItem("New", "test16.png", MenuItemSelect);
                pRoot.Menu.AddItem("Load", "test16.png", MenuItemSelect);
                pRoot.Menu.AddItem("Save", MenuItemSelect);
                pRoot.Menu.AddItem("Save As..", MenuItemSelect);
                pRoot.Menu.AddItem("Quit", MenuItemSelect);
            }

            {
                Control.MenuItem pRoot = menu.AddItem("\u043F\u0438\u0440\u0430\u0442\u0441\u0442\u0432\u043E");
                pRoot.Menu.AddItem("\u5355\u5143\u6D4B\u8BD5", MenuItemSelect);
                pRoot.Menu.AddItem("\u0111\u01A1n v\u1ECB th\u1EED nghi\u1EC7m", "test16.png", MenuItemSelect);
            }

            {
                Control.MenuItem pRoot = menu.AddItem("Submenu");

                Control.MenuItem pCheckable = pRoot.Menu.AddItem("Checkable");
                pCheckable.IsCheckable = true;
                pCheckable.IsCheckable = true;

                {
                    Control.MenuItem pRootB = pRoot.Menu.AddItem("Two");
                    pRootB.Menu.AddItem("Two.One");
                    pRootB.Menu.AddItem("Two.Two");
                    pRootB.Menu.AddItem("Two.Three");
                    pRootB.Menu.AddItem("Two.Four");
                    pRootB.Menu.AddItem("Two.Five");
                    pRootB.Menu.AddItem("Two.Six");
                    pRootB.Menu.AddItem("Two.Seven");
                    pRootB.Menu.AddItem("Two.Eight");
                    pRootB.Menu.AddItem("Two.Nine", "test16.png");
                }

                pRoot.Menu.AddItem("Three");
                pRoot.Menu.AddItem("Four");
                pRoot.Menu.AddItem("Five");

                {
                    Control.MenuItem pRootB = pRoot.Menu.AddItem("Six");
                    pRootB.Menu.AddItem("Six.One");
                    pRootB.Menu.AddItem("Six.Two");
                    pRootB.Menu.AddItem("Six.Three");
                    pRootB.Menu.AddItem("Six.Four");
                    pRootB.Menu.AddItem("Six.Five", "test16.png");

                    {
                        Control.MenuItem pRootC = pRootB.Menu.AddItem("Six.Six");
                        pRootC.Menu.AddItem("Sheep");
                        pRootC.Menu.AddItem("Goose");
                        {
                            Control.MenuItem pRootD = pRootC.Menu.AddItem("Camel");
                            pRootD.Menu.AddItem("Eyes");
                            pRootD.Menu.AddItem("Nose");
                            {
                                Control.MenuItem pRootE = pRootD.Menu.AddItem("Hair");
                                pRootE.Menu.AddItem("Blonde");
                                pRootE.Menu.AddItem("Black");
                                {
                                    Control.MenuItem pRootF = pRootE.Menu.AddItem("Red");
                                    pRootF.Menu.AddItem("Light");
                                    pRootF.Menu.AddItem("Medium");
                                    pRootF.Menu.AddItem("Dark");
                                }
                                pRootE.Menu.AddItem("Brown");
                            }
                            pRootD.Menu.AddItem("Ears");
                        }
                        pRootC.Menu.AddItem("Duck");
                    }

                    pRootB.Menu.AddItem("Six.Seven");
                    pRootB.Menu.AddItem("Six.Eight");
                    pRootB.Menu.AddItem("Six.Nine");
                }

                pRoot.Menu.AddItem("Seven");
            }
        }

        void MenuItemSelect(Base control)
        {
            MenuItem item = control as MenuItem;
            UnitPrint(String.Format("Menu item selected: {0}", item.Text));
        }
    }
}
