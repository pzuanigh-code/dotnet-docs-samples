﻿// Copyright 2020 Google Inc.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using Google.Cloud.PubSub.V1;
using System;
using Xunit;

[Collection(nameof(PubsubFixture))]
public class ListTopicsTest
{
    private readonly PubsubFixture _pubsubFixture;
    private readonly ListProjectTopicsSample _listProjectTopicsSample;
    private readonly CreatePublisherWithServiceCredentialsSample createPublisherWithServiceCredentialsSample;
    public ListTopicsTest(PubsubFixture pubsubFixture)
    {
        _pubsubFixture = pubsubFixture;
        _listProjectTopicsSample = new ListProjectTopicsSample();
        createPublisherWithServiceCredentialsSample =
            new CreatePublisherWithServiceCredentialsSample();
    }

    [Fact]
    public void ListTopics()
    {
        string topicId = "testTopicForListingTopics" + _pubsubFixture.RandomName();
        _pubsubFixture.CreateTopic(topicId);

        _pubsubFixture.Eventually(() =>
        {
            var listProjectTopicsOutput = _listProjectTopicsSample
            .ListProjectTopics(PublisherServiceApiClient.Create(), _pubsubFixture._projectId);
            Assert.Contains(listProjectTopicsOutput, c => c.Contains(topicId));
        });
    }

    [Fact]
    public void ListTopicsWithServiceCredentials()
    {
        string topicId = "testTopicForListingTopics" + _pubsubFixture.RandomName();

        _pubsubFixture.CreateTopic(topicId);

        _pubsubFixture.Eventually(() =>
        {
            var publisher = createPublisherWithServiceCredentialsSample
            .CreatePublisherWithServiceCredentials(Environment.GetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS"));

            var listProjectTopicsOutput = _listProjectTopicsSample
            .ListProjectTopics(publisher, _pubsubFixture._projectId);

            Assert.Contains(listProjectTopicsOutput, c => c.Contains(topicId));
        });
    }
}
