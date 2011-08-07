Global functions in namespaces changed to static classes.
Event namespace removed, C# has built-in event system.
Anim::Think() renamed to Animation.GlobalThink() to avoid name collision.
Base.Hidden -> IsHidden
Base.GetChildren() -> InnerChildren

Get-style functions remained as functions if involving iteration over collections.
Font.size changed to int
fixed: Text/Label not resizing to content
Button: OnPress() -> Pressed()

sfml.net:
MouseButtonEventArgs - added Down field
KeyEventArgs - added Down field

todo:
remove windows.forms and drawing references
refactor once everything is ported (check virtuals, properties, naming consistency, garbage collector stats)
