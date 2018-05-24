import React, { Component } from 'react';
import SuiteResults from './SuiteResults'

class Home extends Component {
  constructor() {
    super();

    this.state = { latestSuiteResults: [] }
  }

  componentDidMount() {
    var getData = () => {
      this.props.getResults()
        .then(response => {
          this.setState({
            latestSuiteResults: response
          })
        })
        .catch(error => alert(error))
    }

    getData();
    this.intervalId = setInterval(() => getData(), 10000);
  }

  componentWillUnmount() {
    clearInterval(this.intervalId);
  }

  render() {
    return (
      <div>
        <h1 className="text-center">ZigNet</h1>
        <SuiteResults suiteResultsGrouped={this.props.grouped} suiteResults={this.state.latestSuiteResults} />
      </div>
    );
  }
}

export default Home;
