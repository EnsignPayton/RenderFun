using RenderFun.Shared;

namespace RenderFun.UI;

public static class VideoDemo
{
    private const int FontIdBody16 = 0;
    private static readonly Color ColorWhite = new(255, 255, 255, 255);

    private static void RenderHeaderButton(string text)
    {
        Clay.UI()
            .Layout(new() { Padding = new(16, 8)})
            .Rectangle(new()
            {
                Color = new(140, 140, 140, 255),
                CornerRadius = new(5)
            })
            .Children(() =>
            {
                Clay.Text(text, new()
                    {
                        FontId = FontIdBody16,
                        FontSize = 16,
                        TextColor = new(255, 255, 255, 255)
                    })
                    .Build();
            })
            .Build();
    }

    private static void RenderDropdownMenuItem(string text)
    {
        Clay.UI()
            .Layout(new() { Padding = new(16, 16) })
            .Children(() =>
            {
                Clay.Text(text, new()
                    {
                        FontId = FontIdBody16,
                        FontSize = 16,
                        TextColor = new(255, 255, 255, 255)
                    })
                    .Build();
            })
            .Build();
    }

    private record struct Document(string Title, string Contents);

    private static readonly Document[] Documents =
    [
        new("Squirrels",
            """
            The Secret Life of Squirrels: Nature's Clever Acrobats
            Squirrels are often overlooked creatures, dismissed as mere park inhabitants or backyard nuisances. Yet, beneath their fluffy tails and twitching noses lies an intricate world of cunning, agility, and survival tactics that are nothing short of fascinating. As one of the most common mammals in North America, squirrels have adapted to a wide range of environments from bustling urban centers to tranquil forests and have developed a variety of unique behaviors that continue to intrigue scientists and nature enthusiasts alike.

            Master Tree Climbers
            At the heart of a squirrel's skill set is its impressive ability to navigate trees with ease. Whether they're darting from branch to branch or leaping across wide gaps, squirrels possess an innate talent for acrobatics. Their powerful hind legs, which are longer than their front legs, give them remarkable jumping power. With a tail that acts as a counterbalance, squirrels can leap distances of up to ten times the length of their body, making them some of the best aerial acrobats in the animal kingdom.
            But it's not just their agility that makes them exceptional climbers. Squirrels' sharp, curved claws allow them to grip tree bark with precision, while the soft pads on their feet provide traction on slippery surfaces. Their ability to run at high speeds and scale vertical trunks with ease is a testament to the evolutionary adaptations that have made them so successful in their arboreal habitats.

            Food Hoarders Extraordinaire
            Squirrels are often seen frantically gathering nuts, seeds, and even fungi in preparation for winter. While this behavior may seem like instinctual hoarding, it is actually a survival strategy that has been honed over millions of years. Known as "scatter hoarding," squirrels store their food in a variety of hidden locations, often burying it deep in the soil or stashing it in hollowed-out tree trunks.
            Interestingly, squirrels have an incredible memory for the locations of their caches. Research has shown that they can remember thousands of hiding spots, often returning to them months later when food is scarce. However, they don't always recover every stash some forgotten caches eventually sprout into new trees, contributing to forest regeneration. This unintentional role as forest gardeners highlights the ecological importance of squirrels in their ecosystems.

            The Great Squirrel Debate: Urban vs. Wild
            While squirrels are most commonly associated with rural or wooded areas, their adaptability has allowed them to thrive in urban environments as well. In cities, squirrels have become adept at finding food sources in places like parks, streets, and even garbage cans. However, their urban counterparts face unique challenges, including traffic, predators, and the lack of natural shelters. Despite these obstacles, squirrels in urban areas are often observed using human infrastructure such as buildings, bridges, and power lines as highways for their acrobatic escapades.
            There is, however, a growing concern regarding the impact of urban life on squirrel populations. Pollution, deforestation, and the loss of natural habitats are making it more difficult for squirrels to find adequate food and shelter. As a result, conservationists are focusing on creating squirrel-friendly spaces within cities, with the goal of ensuring these resourceful creatures continue to thrive in both rural and urban landscapes.

            A Symbol of Resilience
            In many cultures, squirrels are symbols of resourcefulness, adaptability, and preparation. Their ability to thrive in a variety of environments while navigating challenges with agility and grace serves as a reminder of the resilience inherent in nature. Whether you encounter them in a quiet forest, a city park, or your own backyard, squirrels are creatures that never fail to amaze with their endless energy and ingenuity.
            In the end, squirrels may be small, but they are mighty in their ability to survive and thrive in a world that is constantly changing. So next time you spot one hopping across a branch or darting across your lawn, take a moment to appreciate the remarkable acrobat at work a true marvel of the natural world.

            """),
        new("Lorem Ipsum",
            "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum."),
        new("Vacuum Instructions",
            """
            Chapter 3: Getting Started - Unpacking and Setup

            Congratulations on your new SuperClean Pro 5000 vacuum cleaner! In this section, we will guide you through the simple steps to get your vacuum up and running. Before you begin, please ensure that you have all the components listed in the "Package Contents" section on page 2.

            1. Unboxing Your Vacuum
            Carefully remove the vacuum cleaner from the box. Avoid using sharp objects that could damage the product. Once removed, place the unit on a flat, stable surface to proceed with the setup. Inside the box, you should find:
            
                The main vacuum unit
                A telescoping extension wand
                A set of specialized cleaning tools (crevice tool, upholstery brush, etc.)
                A reusable dust bag (if applicable)
                A power cord with a 3-prong plug
                A set of quick-start instructions

            2. Assembling Your Vacuum
            Begin by attaching the extension wand to the main body of the vacuum cleaner. Line up the connectors and twist the wand into place until you hear a click. Next, select the desired cleaning tool and firmly attach it to the wand's end, ensuring it is securely locked in.

            For models that require a dust bag, slide the bag into the compartment at the back of the vacuum, making sure it is properly aligned with the internal mechanism. If your vacuum uses a bagless system, ensure the dust container is correctly seated and locked in place before use.

            3. Powering On
            To start the vacuum, plug the power cord into a grounded electrical outlet. Once plugged in, locate the power switch, usually positioned on the side of the handle or body of the unit, depending on your model. Press the switch to the "On" position, and you should hear the motor begin to hum. If the vacuum does not power on, check that the power cord is securely plugged in, and ensure there are no blockages in the power switch.

            Note: Before first use, ensure that the vacuum filter (if your model has one) is properly installed. If unsure, refer to "Section 5: Maintenance" for filter installation instructions.
            """),
        new("Article 4", "Article 4"),
        new("Article 5", "Article 5"),
    ];

    private static uint SelectedDocumentIndex;

    // private static void HandleSidebarInteraction()

    public static void Compute()
    {
        var layoutExpand = new Sizing
        {
            Width = Clay.SizingGrow(),
            Height = Clay.SizingGrow()
        };

        var contentBackgroundConfig = new RectangleElementConfig
        {
            Color = new(90, 90, 90, 255),
            CornerRadius = new(8)
        };

        Clay.UI()
            .Id("OuterContainer")
            .Rectangle(new() { Color = new(43, 41, 51, 255) })
            .Layout(new()
            {
                LayoutDirection = LayoutDirection.TopToBottom,
                Sizing = layoutExpand,
                Padding = new(16, 16),
                ChildGap = 16
            })
            .Children(() =>
            {
                Clay.UI()
                    .Id("HeaderBar")
                    .Rectangle(contentBackgroundConfig)
                    .Layout(new()
                    {
                        Sizing = new()
                        {
                            Height = Clay.SizingFixed(60),
                            Width = Clay.SizingGrow()
                        },
                        Padding = new(16, 0),
                        ChildGap = 16,
                        ChildAlignment = new()
                        {
                            Y = LayoutAlignmentY.Center
                        }
                    })
                    .Children(() =>
                    {
                        Clay.UI()
                            .Id("FileButton")
                            .Layout(new() { Padding = new(16, 8) })
                            .Rectangle(new()
                            {
                                Color = new(140, 140, 140, 255),
                                CornerRadius = new(5)
                            })
                            .Children(() =>
                            {
                                Clay.Text("File")
                                    .Config(new()
                                    {
                                        FontId = FontIdBody16,
                                        FontSize = 16,
                                        TextColor = new(255, 255, 255, 255)
                                    })
                                    .Build();

                                // TODO: PointerOver / GetElementId
                                bool fileMenuVisible = true;
                                if (fileMenuVisible)
                                {
                                    Clay.UI()
                                        .Id("FileMenu")
                                        .Floating(new()
                                        {
                                            Attachment = new()
                                            {
                                                Parent = FloatingAttachPointType.LeftBottom
                                            }
                                        })
                                        .Layout(new()
                                        {
                                            Padding = new(0, 8)
                                        })
                                        .Children(() =>
                                        {
                                            Clay.UI()
                                                .Layout(new()
                                                {
                                                    LayoutDirection = LayoutDirection.TopToBottom,
                                                    Sizing = new()
                                                    {
                                                        Width = Clay.SizingFixed(200)
                                                    }
                                                })
                                                .Rectangle(new()
                                                {
                                                    Color = new(40, 40, 40, 255),
                                                    CornerRadius = new(8)
                                                })
                                                .Children(() =>
                                                {
                                                    RenderDropdownMenuItem("New");
                                                    RenderDropdownMenuItem("Open");
                                                    RenderDropdownMenuItem("Close");
                                                })
                                                .Build();
                                        })
                                        .Build();
                                }
                            })
                            .Build();

                        RenderHeaderButton("Edit");

                        Clay.UI()
                            .Layout(new() { Sizing = new() { Width = Clay.SizingGrow() } })
                            .Build();

                        RenderHeaderButton("Upload");
                        RenderHeaderButton("Media");
                        RenderHeaderButton("Support");
                    })
                    .Build();

                Clay.UI()
                    .Id("LowerContent")
                    .Layout(new() { Sizing = layoutExpand, ChildGap = 16 })
                    .Children(() =>
                    {
                        Clay.UI()
                            .Id("Sidebar")
                            .Rectangle(contentBackgroundConfig)
                            .Layout(new()
                            {
                                LayoutDirection = LayoutDirection.TopToBottom,
                                Padding = new(16, 16),
                                ChildGap = 8,
                                Sizing = new()
                                {
                                    Width = Clay.SizingFixed(250),
                                    Height = Clay.SizingGrow()
                                }
                            })
                            .Children(() =>
                            {
                                for (int i = 0; i < Documents.Length; i++)
                                {
                                    var document = Documents[i];

                                    var sidebarButtonLayout = new LayoutConfig
                                    {
                                        Sizing = new() { Width = Clay.SizingGrow() },
                                        Padding = new(16, 16)
                                    };

                                    if (i == SelectedDocumentIndex)
                                    {
                                        Clay.UI()
                                            .Layout(sidebarButtonLayout)
                                            .Rectangle(new()
                                            {
                                                Color = new(120, 120, 120, 255),
                                                CornerRadius = new(8)
                                            })
                                            .Children(() =>
                                            {
                                                Clay.Text(document.Title)
                                                    .Config(new()
                                                    {
                                                        FontId = FontIdBody16,
                                                        FontSize = 20,
                                                        TextColor = new(255, 255, 255, 255)
                                                    })
                                                    .Build();
                                            })
                                            .Build();
                                    }
                                    else
                                    {
                                        Clay.UI()
                                            .Layout(sidebarButtonLayout)
                                            // TODO: OnHover
                                            .Children(() =>
                                            {
                                                Clay.Text(document.Title)
                                                    .Config(new()
                                                    {
                                                        FontId = FontIdBody16,
                                                        FontSize = 20,
                                                        TextColor = new(255, 255, 255, 255)
                                                    })
                                                    .Build();
                                            })
                                            .Build();
                                    }
                                }
                            })
                            .Build();

                        Clay.UI()
                            .Id("MainContent")
                            .Rectangle(contentBackgroundConfig)
                            // TODO: Scroll
                            //.WithScroll()
                            .Layout(new()
                            {
                                LayoutDirection = LayoutDirection.TopToBottom,
                                ChildGap = 16,
                                Padding = new(16, 16),
                                Sizing = layoutExpand
                            })
                            .Children(() =>
                            {
                                var selectedDocument = Documents[SelectedDocumentIndex];
                                Clay.Text(selectedDocument.Title)
                                    .Config(new()
                                    {
                                        FontId = FontIdBody16,
                                        FontSize = 24,
                                        TextColor = ColorWhite
                                    })
                                    .Build();

                                Clay.Text(selectedDocument.Contents)
                                    .Config(new()
                                    {
                                        FontId = FontIdBody16,
                                        FontSize = 24,
                                        TextColor = ColorWhite
                                    })
                                    .Build();
                            })
                            .Build();
                    })
                    .Build();
            })
            .Build();
    }
}