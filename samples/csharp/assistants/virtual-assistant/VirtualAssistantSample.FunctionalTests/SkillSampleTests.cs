﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Threading.Tasks;
using Microsoft.Bot.Connector.DirectLine;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VirtualAssistantSample.Models;
using VirtualAssistantSample.Tests;
using VirtualAssistantSample.Tests.Utterances;

namespace VirtualAssistantSample.FunctionalTests
{
    [TestClass]
    [TestCategory("FunctionalTests")]
    [TestCategory("SkillSample")]
    public class SkillSampleTests : DirectLineClientTestBase
    {
        [TestMethod]
        public async Task Test_Sample_Utterance()
        {
            await Assert_Utterance_Triggers_SkillSample();
        }

        /// <summary>
        /// Assert that a connected SkillSample is triggered by a sample utterance and completes the VA dialog.
        /// </summary>
        /// <returns>Task.</returns>
        public async Task Assert_Utterance_Triggers_SkillSample()
        {
            var profileState = new UserProfileState { Name = TestName };
            var namePromptVariations = AllResponsesTemplates.ExpandTemplate("NamePrompt");
            var haveNameMessageVariations = AllResponsesTemplates.ExpandTemplate("HaveNameMessage", profileState);
            var completedMessageVariations = AllResponsesTemplates.ExpandTemplate("CompletedMessage");

            var conversation = await new CoreTests().Assert_New_User_Greeting();

            // Assert Skill is triggered by sample utterance
            var responses = await SendActivityAsync(conversation, CreateMessageActivity(GeneralUtterances.SkillSample));
            CollectionAssert.Contains(namePromptVariations as ICollection, responses[2].Text);

            responses = await SendActivityAsync(conversation, CreateMessageActivity(TestName));
            CollectionAssert.Contains(haveNameMessageVariations as ICollection, responses[3].Text);

            // Assert dialog has completed
            CollectionAssert.Contains(completedMessageVariations as ICollection, responses[4].Text);
        }
    }
}