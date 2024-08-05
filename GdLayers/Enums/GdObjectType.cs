using GdLayers.Attributes;
using GdLayers.Constants;

namespace GdLayers.Enums;

public enum ObjectType : byte
{
    // blocks
    [Title("Solid Blocks")]
    [ResourceReference(ResourceTypeConstants.GdObjectGroup.Image, "SolidBlocks")]
    [ResourceReference(ResourceTypeConstants.GdObjectGroup.Description, "SolidBlocks_Description")]
    SolidBlock,

    [Title("Non-Solid Blocks")]
    [ResourceReference(ResourceTypeConstants.GdObjectGroup.Image, "NonSolidBlocks")]
    [ResourceReference(ResourceTypeConstants.GdObjectGroup.Description, "NonSolidBlocks_Description")]
    NonSolidBlock,

    // spikes
    [Title("Solid Spikes")]
    [ResourceReference(ResourceTypeConstants.GdObjectGroup.Image, "SolidSpikes")]
    [ResourceReference(ResourceTypeConstants.GdObjectGroup.Description, "SolidSpikes_Description")]
    SolidSpike,

    [Title("Non-Solid Spikes")]
    [ResourceReference(ResourceTypeConstants.GdObjectGroup.Image, "NonSolidSpikes")]
    [ResourceReference(ResourceTypeConstants.GdObjectGroup.Description, "NonSolidSpikes_Description")]
    NonSolidSpike,

    // hazards
    [Title("Animated Hazards")]
    [ResourceReference(ResourceTypeConstants.GdObjectGroup.Image, "AnimatedHazards")]
    [ResourceReference(ResourceTypeConstants.GdObjectGroup.Description, "AnimatedHazards_Description")]
    AnimatedHazard,

    [Title("Other Hazards")]
    [ResourceReference(ResourceTypeConstants.GdObjectGroup.Image, "OtherHazards")]
    [ResourceReference(ResourceTypeConstants.GdObjectGroup.Description, "OtherHazards_Description")]
    OtherHazard,

    // deco
    [Title("Outline Decorations")]
    [ResourceReference(ResourceTypeConstants.GdObjectGroup.Image, "OutlineDecorations")]
    [ResourceReference(ResourceTypeConstants.GdObjectGroup.Description, "OutlineDecorations_Description")]
    OutlineDecoration,

    [Title("Filler Decorations")]
    [ResourceReference(ResourceTypeConstants.GdObjectGroup.Image, "FillerDecorations")]
    [ResourceReference(ResourceTypeConstants.GdObjectGroup.Description, "FillerDecorations_Description")]
    FillerDecoration,

    [Title("Animated Decorations")]
    [ResourceReference(ResourceTypeConstants.GdObjectGroup.Image, "AnimatedDecorations")]
    [ResourceReference(ResourceTypeConstants.GdObjectGroup.Description, "AnimatedDecorations_Description")]
    AnimatedDecoration,

    [Title("Pixel Decorations")]
    [ResourceReference(ResourceTypeConstants.GdObjectGroup.Image, "PixelDecorations")]
    [ResourceReference(ResourceTypeConstants.GdObjectGroup.Description, "PixelDecorations_Description")]
    PixelDecoration,

    [Title("Animated Pixel Decorations")]
    [ResourceReference(ResourceTypeConstants.GdObjectGroup.Image, "AnimatedPixelDecorations")]
    [ResourceReference(ResourceTypeConstants.GdObjectGroup.Description, "AnimatedPixelDecorations_Description")]
    AnimatedPixelDecoration,

    [Title("Glow Decorations")]
    [ResourceReference(ResourceTypeConstants.GdObjectGroup.Image, "GlowDecorations")]
    [ResourceReference(ResourceTypeConstants.GdObjectGroup.Description, "GlowDecorations_Description")]
    GlowDecoration,

    [Title("Other Decorations")]
    [ResourceReference(ResourceTypeConstants.GdObjectGroup.Image, "OtherDecorations")]
    [ResourceReference(ResourceTypeConstants.GdObjectGroup.Description, "OtherDecorations_Description")]
    OtherDecoration,

    // gameplay
    [Title("Launching Pads")]
    [ResourceReference(ResourceTypeConstants.GdObjectGroup.Image, "LaunchingPads")]
    [ResourceReference(ResourceTypeConstants.GdObjectGroup.Description, "LaunchingPads_Description")]
    LaunchingPad,

    [Title("Orbs")]
    [ResourceReference(ResourceTypeConstants.GdObjectGroup.Image, "Orbs")]
    [ResourceReference(ResourceTypeConstants.GdObjectGroup.Description, "Orbs_Description")]
    Orb,

    [Title("Gravity Portals")]
    [ResourceReference(ResourceTypeConstants.GdObjectGroup.Image, "GravityPortals")]
    [ResourceReference(ResourceTypeConstants.GdObjectGroup.Description, "GravityPortals_Description")]
    GravityPortal,

    [Title("Gamemode Portals")]
    [ResourceReference(ResourceTypeConstants.GdObjectGroup.Image, "GamemodePortals")]
    [ResourceReference(ResourceTypeConstants.GdObjectGroup.Description, "GamemodePortals_Description")]
    GamemodePortal,

    [Title("Size Portals")]
    [ResourceReference(ResourceTypeConstants.GdObjectGroup.Image, "SizePortals")]
    [ResourceReference(ResourceTypeConstants.GdObjectGroup.Description, "SizePortals_Description")]
    SizePortal,

    [Title("Other Portals")]
    [ResourceReference(ResourceTypeConstants.GdObjectGroup.Image, "OtherPortals")]
    [ResourceReference(ResourceTypeConstants.GdObjectGroup.Description, "OtherPortals_Description")]
    OtherPortal,

    // other gameplay
    [Title("Speed Modifiers")]
    [ResourceReference(ResourceTypeConstants.GdObjectGroup.Image, "SpeedModifiers")]
    [ResourceReference(ResourceTypeConstants.GdObjectGroup.Description, "SpeedModifiers_Description")]
    SpeedModifier,

    [Title("Special Blocks")]
    [ResourceReference(ResourceTypeConstants.GdObjectGroup.Image, "SpecialBlocks")]
    [ResourceReference(ResourceTypeConstants.GdObjectGroup.Description, "SpecialBlocks_Description")]
    SpecialBlock,

    // saw
    [Title("Saws")]
    [ResourceReference(ResourceTypeConstants.GdObjectGroup.Image, "Saws")]
    [ResourceReference(ResourceTypeConstants.GdObjectGroup.Description, "Saws_Description")]
    Saw,

    // other
    [Title("Collectible Objects")]
    [ResourceReference(ResourceTypeConstants.GdObjectGroup.Image, "Collectibles")]
    [ResourceReference(ResourceTypeConstants.GdObjectGroup.Description, "Collectibles_Description")]
    Collectible,

    [Title("Triggers")]
    [ResourceReference(ResourceTypeConstants.GdObjectGroup.Image, "Triggers")]
    [ResourceReference(ResourceTypeConstants.GdObjectGroup.Description, "Triggers_Description")]
    Trigger
}
